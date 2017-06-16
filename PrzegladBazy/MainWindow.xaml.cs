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
    public partial class MainWindow : INotifyPropertyChanged
    {
        public ObservableCollection<SlownikGroup> Groups = new ObservableCollection<SlownikGroup>();
        /// <summary>
        /// Dostęp do danych z bazy
        /// </summary>
        private readonly wizualizacja2Entities1 _context = new wizualizacja2Entities1();
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
        /// <summary>
        /// Ciąg znaków definiujący połączenie z bazą danych.
        /// </summary>
        public string ConnectionString;
        /// <summary>
        /// Zdarzenie wywoływane każdorazowo przy zmianie którejś z właściwości klasy.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            grupySlownikow.ItemsSource = Groups;
            // Sprawdz Poprawnosć połączenia z bazą danych
            Task.Run(() => TestDbConnections());
        }
        /// <summary>
        /// Funkcja testująca poprawność połączenia z bazą danych, a co za tym idzie
        /// zmienia właściwości kontrolek, tak aby niemożliwe było wykoanie operacji 
        /// na bazie danych.
        /// </summary>
        private void TestDbConnections()
        {
            // Zablokuj elementy które pozwalają na wykonanie operacji na bazie danych
            Dispatcher.Invoke(() =>
            {
                slowniki.IsEnabled = false;
                grupySlownikow.IsEnabled = false;
                slowniki.DropDownClosed -= Slowniki_DropDownClosed;
                grupySlownikow.DropDownClosed -= GrupySlownikow_OnDropDownClosed;
            });
            
            try
            {
                // Pobierz słowniki z bazy danych
                Slownik = _context.Slownik.First();
                var foo = (from d in _context.Slownik orderby d.LongGate select d.LongGate).ToList();

                // Zaktualizuj interfejs graficzny
                Dispatcher.Invoke(() =>
                {
                    slowniki.ItemsSource = foo;

                    slowniki.IsEnabled = true;
                    grupySlownikow.IsEnabled = true;

                    slowniki.DropDownClosed += Slowniki_DropDownClosed;
                    grupySlownikow.DropDownClosed += GrupySlownikow_OnDropDownClosed;
                });
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);
            }
        }
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
            
            // Lista tymczasowa do punktów wykresu.
            var points = MakeDataSeries((string)slowniki.SelectedValue);

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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGroups();
        }
        /// <summary>
        /// Funkcja ta ładuje grupy, które zostały zapisane na dysku.
        /// </summary>
        private void LoadGroups()
        {
            const string path = @".\Groups\";
            var files = Directory.GetFiles(path, "*.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(SlownikGroup));
            foreach (var xml in files)
            {
                StreamReader reader = new StreamReader(xml);
                Groups.Add((SlownikGroup)serializer.Deserialize(reader));
                reader.Close();
            }
        }
        /// <summary>
        /// Reakcja na naciśnięcie przycisku - powoduje otwaracie okna w którym możliwe jest 
        /// stworzenie nowej grupy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateGroup_OnClick(object sender, RoutedEventArgs e)
        {
            var okno = new CreateGroup(this);
            okno.Show();
        }
        /// <summary>
        /// Reakcja na zamknięcie listy rozwijanej wyboru grup słowników.
        /// 
        /// Zamknięcie jest równoznaczne z załadowaniem całej grupy do pamięci i wyrysowanie
        /// wykresu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrupySlownikow_OnDropDownClosed(object sender, EventArgs e)
        {
            // Jeśli nic nie wybrano
            if (grupySlownikow.SelectedValue == null)
                return;

            // Odnajdź wszystkie słowniki z grupy w bazie.
            var grupa = grupySlownikow.SelectionBoxItem as SlownikGroup;

            if (grupa == null)
                return;

            // Wygląd wykresu
            var model = new PlotModel { Title = grupa._title };
            model.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Right });

            // tworzenie serii danych
            foreach (var grp in grupa._slowniki)
            {
                var points = MakeDataSeries(grp);

                // Seria wykresu
                model.Series.Add(new OxyPlot.Series.LineSeries { ItemsSource = points });
            }
            Model1.Series.Clear();
            // Aktualizacja wykresu.
            Model1 = model;
        }
        /// <summary>
        /// Metoda która na podstawie LongID (odnoszącego się do słownika) zwraca listę
        /// punktów pobranych z bazy danych.
        /// </summary>
        /// <param name="longGate"></param>
        /// <returns></returns>
        private List<DataPoint> MakeDataSeries(string longGate)
        {
            var slownik = _context.Slownik.First(d => d.LongGate == longGate);

            // Lista tymczasowa do punktów wykresu.
            var points = new List<DataPoint>();
            if (data.SelectedDate == null)
                return points;

            var dataa = (DateTime) data.SelectedDate;
            var timesp = dataa.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var timesp1 = dataa.Subtract(new DateTime(1970, 1, 2)).TotalMilliseconds;
            
            // Utwórz punkty wykresu dla każdego odnalezionego pomiaru
            foreach (var foo in (from d in _context.wartosc_bramek
                where d.gateId == slownik.gateId
                      && d.time < timesp && d.time > timesp1
                orderby d.time
                select d))
            {
                points.Add(new DataPoint(
                    OxyPlot.Axes.DateTimeAxis.ToDouble(new DateTime(1970, 1, 1).AddMilliseconds(foo.time)),
                    /*Slownik.rodzajPomiaru.Equals("D", StringComparison.OrdinalIgnoreCase) ? foo.value < 1 ? 0 : 1 : foo.value*/
                    foo.value
                ));
            }
            return points;
        }
        /// <summary>
        /// Funkcja ta powoduje zmianę połączenia bazy danych.
        /// </summary>
        /// <param name="server">Nazwa serwera</param>
        /// <param name="databaseName">Nazwa bazy danych</param>
        /// <param name="userName">Użytkownik</param>
        /// <param name="password">Hasło</param>
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
            
            _context.Database.Connection.ConnectionString = ConnectionString = builder.ConnectionString;
            TestDbConnections();
        }
        /// <summary>
        /// Reakcja na naciśnięcie przycisku - powoduje otwarcie nowego okna, gdzie możliwa jest 
        /// zmiana parametrów połączenia z bazą danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtChangeConnection_OnClick(object sender, RoutedEventArgs e)
        {
            (new DBUser(this)).Show();
        }
    }
}
