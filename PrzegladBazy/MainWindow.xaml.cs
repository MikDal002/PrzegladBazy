using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using PrzegladBazy.Annotations;
using PrzegladBazy.Models;

namespace PrzegladBazy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<SlownikGroup> Groups = new ObservableCollection<SlownikGroup>();
        /// <summary>
        /// Dostęp do danych z bazy
        /// </summary>
        private wizualizacja2Entities1 _context = new wizualizacja2Entities1();
        /// <summary>
        /// Reprezentuje aktualnie wybrane słowo z słownika
        /// </summary>
        public Slownik Slownik
        {
            get => _slownik;
            set
            {
                if (value != _slownik)
                {
                    _slownik = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private Slownik _slownik = new Slownik();
        /// <summary>
        /// Reperezentuje wykres na głównej stornie
        /// </summary>
        public PlotModel Model1
        {
            get => _model1;
            set
            {
                if (_model1 != value)
                {
                    _model1 = value; NotifyPropertyChanged();
                }
            }
        }
        private PlotModel _model1 = new PlotModel();
        public string connectionString;
        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            grupySlownikow.ItemsSource = Groups;
            Task.Run(() => testDBConnections());

        }

        private void testDBConnections()
        {
            Dispatcher.Invoke(() =>
            {
                slowniki.IsEnabled = false;
                grupySlownikow.IsEnabled = false;
                slowniki.DropDownClosed -= Slowniki_DropDownClosed;
                grupySlownikow.DropDownClosed -= GrupySlownikow_OnDropDownClosed;
            });
            

            try
            {
                _context.Database.Connection.Open();
                Dispatcher.Invoke(() =>
                {
                    slowniki.DropDownClosed += Slowniki_DropDownClosed;
                    grupySlownikow.DropDownClosed += GrupySlownikow_OnDropDownClosed;
                    var foo = (from d in _context.Slownik orderby d.LongGate select d.LongGate).ToList();

                    slowniki.ItemsSource = foo;

                    slowniki.IsEnabled = true;
                    grupySlownikow.IsEnabled = true;
                });
                
                
                this.Slownik = _context.Slownik.First();


                

            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                _context.Database.Connection.Close();
            }


            
        }

        /// <summary>
        /// Zdarzenie wywoływane każdorazowo przy zmianie którejś z właściwości klasy.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Funkcja wywołująca zdarzenie PropertyChanged.
        /// </summary>
        /// <param name="name"></param>
        private void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Rekacja na zamknięcie okna listy wybieralnej. Wywoływany jest np zaraz po wybraniu,
        /// którejś z pozycji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slowniki_DropDownClosed(object sender, EventArgs e)
        {
            // Jeśli nic nie wybrano
            if (slowniki.SelectedValue == null)
                return;

            // Odnajdź wybraną pozycję w bazie.
            Slownik = _context.Slownik.First(d => d.LongGate == (string)slowniki.SelectedValue);
            
            // Lista tymczasowa do punktów wykresu.
            var points = new List<DataPoint>();
            if (data.SelectedDate == null)
                return;
            var dataa = (DateTime)data.SelectedDate;
            var timesp = dataa.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var timesp1 = dataa.Subtract(new DateTime(1970, 1, 2)).TotalMilliseconds;
            // Utwórz punkty wykresu dla każdego odnalezionego pomiaru
            foreach (var foo in (from d in _context.wartosc_bramek
                where d.gateId == Slownik.gateId
                      && d.time < timesp && d.time > timesp1
                orderby d.time
                select d))
            {
                points.Add(new DataPoint(
                    OxyPlot.Axes.DateTimeAxis.ToDouble(new DateTime(1970, 1, 1).AddMilliseconds(foo.time)),
                    Slownik.rodzajPomiaru.Equals("D", StringComparison.OrdinalIgnoreCase) ? foo.value < 1 ? 0 : 1 : foo.value
                    ));
            }

            // Wygląd wykresu
            var model = new PlotModel { Title = Slownik.LongGate };
            model.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Right });

            // Seria wykresu
            model.Series.Add(new OxyPlot.Series.LineSeries { ItemsSource = points });
            Model1.Series.Clear();
            // Aktualizacja wykresu.
            Model1 = model;
        }
        /// <summary>
        /// Funkcja wywoływana zaraz po załadowaniu okna. 
        /// Ładuje ona wszystkie dostępne dane z bazy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane z bazy
            //await _context.Slownik.LoadAsync();
            //await _context.pomiary.LoadAsync();

            // Znajdz pierwszy wpis w słowniku, aby coś załadować
            

            // Załaduj grupy z plików
            loadGroups();

            
        }

        private void loadGroups()
        {
            List<string> grp = null;
            string path = @".\Groups\";
            var files = Directory.GetFiles(path, "*.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(SlownikGroup));
            foreach (var xml in files)
            {
                StreamReader reader = new StreamReader(xml);
                Groups.Add((SlownikGroup)serializer.Deserialize(reader));
                reader.Close();
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var okno = new CreateGroup(this);
            okno.Show();
        }

        private void GrupySlownikow_OnDropDownClosed(object sender, EventArgs e)
        {
            // Jeśli nic nie wybrano
            if (grupySlownikow.SelectedValue == null)
                return;

            // Odnajdź wszystkie słowniki z grupy w bazie.
            var grupa = grupySlownikow.SelectionBoxItem as SlownikGroup;
            List<Slownik> grupaPrSlow = new List<Slownik>();

            // Wygląd wykresu
            var model = new PlotModel { Title = grupa._title};
            model.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Right });

            // tworzenie serii danych
            foreach (var grp in grupa._slowniki)
            {
                Slownik Slownik = _context.Slownik.First(d => d.LongGate == grp);

                // Lista tymczasowa do punktów wykresu.
                var points = new List<DataPoint>();
                if (data.SelectedDate == null)
                    return;

                var dataa = (DateTime)data.SelectedDate;
                var timesp = dataa.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                var timesp1 = dataa.Subtract(new DateTime(1970, 1, 2)).TotalMilliseconds;
                // Utwórz punkty wykresu dla każdego odnalezionego pomiaru
                foreach (var foo in (from d in _context.wartosc_bramek
                                     where d.gateId == Slownik.gateId
                                     && d.time < timesp && d.time > timesp1
                                     orderby d.time select d))
                {
                    points.Add(new DataPoint(
                        OxyPlot.Axes.DateTimeAxis.ToDouble(new DateTime(1970, 1, 1).AddMilliseconds(foo.time)),
                        /*Slownik.rodzajPomiaru.Equals("D", StringComparison.OrdinalIgnoreCase) ? foo.value < 1 ? 0 : 1 : foo.value*/ foo.value
                    ));
                }

                // Seria wykresu
                model.Series.Add(new OxyPlot.Series.LineSeries { ItemsSource = points });
            }
            Model1.Series.Clear();
            // Aktualizacja wykresu.
            Model1 = model;
        }

        private DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public void ChangeDatabaseConnection(string server, string databaseName, string userName, string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server, // server address
                InitialCatalog = databaseName, // database name
                IntegratedSecurity = false, // server auth(false)/win auth(true)
                MultipleActiveResultSets = false, // activate/deactivate MARS
                PersistSecurityInfo = true, // hide login credentials
                UserID = userName, // user name
                Password = password // password
            };
            
            _context.Database.Connection.ConnectionString = connectionString = builder.ConnectionString;
            testDBConnections();
        }


        private void BtChangeConnection_OnClick(object sender, RoutedEventArgs e)
        {
            (new DBUser(this)).Show();
        }
    }
    [Serializable]
    public class SlownikGroup
    {
        public List<string> _slowniki = new List<string>();
        public string _title = null;
        

        public SlownikGroup()
        {
            _title = "noname";
        }

        public override string ToString()
        {
            return _title;
        }
    }

    
}
