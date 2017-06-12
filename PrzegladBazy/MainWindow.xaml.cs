using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
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
        private List<List<string>> groups = new List<List<string>>();
        /// <summary>
        /// Dostęp do danych z bazy
        /// </summary>
        private wizualizacja2Entities _context = new wizualizacja2Entities();
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
        /// Konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            slowniki.DropDownClosed += Slowniki_DropDownClosed;
            slowniki.IsEnabled = false;
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
            Slownik = _context.Slownik.Local.First(d => d.LongGate == (string)slowniki.SelectedValue);
            
            // Lista tymczasowa do punktów wykresu.
            var points = new List<DataPoint>();

            // Utwórz punkty wykresu dla każdego odnalezionego pomiaru
            foreach (var foo in _context.pomiary.Local.Where(d => d.gateId == Slownik.gateId))
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
            await _context.Slownik.LoadAsync();
            await _context.pomiary.LoadAsync();

            // Znajdz pierwszy wpis w słowniku, aby coś załadować
            this.Slownik = _context.Slownik.Local.First();

           
            var foo = (from d in _context.Slownik where _context.pomiary.Any(ed => ed.gateId == d.gateId) select d.LongGate).ToList();
            foo.Sort();
            slowniki.ItemsSource = foo;

            slowniki.IsEnabled = true;
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var okno = new CreateGroup(this);
            okno.Show();
        }
    }

    public class SlownikGroup
    {
        private List<Slownik> _slowniki = null;

        public SlownikGroup(IList<Slownik> slowniki)
        {
            _slowniki = slowniki.ToList();
        }
    }
}
