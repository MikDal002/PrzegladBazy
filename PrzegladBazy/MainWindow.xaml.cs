using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
        private wizualizacja2Entities _context = new wizualizacja2Entities();

        private Slownik _slownik = new Slownik();
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

        private IList<DataPoint> _points = new List<DataPoint>
        {
            new DataPoint(0, 4),
            new DataPoint(10, 13),
            new DataPoint(20, 15),
            new DataPoint(30, 16),
            new DataPoint(40, 12),
            new DataPoint(50, 12)
        };
        public IList<DataPoint> Points
        {
            get => _points;
            set
            {
                if (value != _points)
                {
                    _points = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            slowniki.DropDownClosed += Slowniki_DropDownClosed;
            //var siema = new MainViewModel();
            //chart.Model = siema.MyModel;
        }

        private void Slowniki_DropDownClosed(object sender, EventArgs e)
        {
            
            Slownik = _context.Slownik.Local.First(d => d.LongGate == (string)slowniki.SelectedValue);
            var dupa = _context.pomiary.Local.Where(d => d.gateId == Slownik.gateId).ToList();
            
            Debug.WriteLine(dupa.Count);
            var points = new List<DataPoint>();
            int i = 0;
            foreach (var foo in dupa)
            {
                points.Add(new DataPoint(i++, foo.value));
            }

            Points = points;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        async private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            await _context.Slownik.LoadAsync();
            this.Slownik = _context.Slownik.Local.First();
            _context.pomiary.Load();
            var foo = (from d in _context.Slownik.Local select d.LongGate).ToList();
            foo.Sort();
            slowniki.ItemsSource = foo;
            //System.Windows.Data.CollectionViewSource slownikViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("slownikViewSource")));
            //// Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            //// slownikViewSource.Źródło = [ogólne źródło danych]

            //_context.Slownik.Load();
            //this.Slownik = _context.Slownik.Local.First();


            //_context.pomiary.Where(abcd => abcd.gateId == 6554072).Load();

            //var ziup = _context.pomiary.Local;
            //var i = 0;
        }
    }
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Title = "Example 2";
            this.Points = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };
        }

        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
}
