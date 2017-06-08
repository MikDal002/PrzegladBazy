using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
using PrzegladBazy.Models;

namespace PrzegladBazy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private wizualizacja2Entities _context = new wizualizacja2Entities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
        System.Windows.Data.CollectionViewSource slownikViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("slownikViewSource")));
        // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
        // slownikViewSource.Źródło = [ogólne źródło danych]

        _context.Slownik.Load();
            slownikViewSource.Source = _context.Slownik.Local;

            _context.pomiary.Where(abcd => abcd.gateId == 6554072).Load();

            var ziup = _context.pomiary.Local;
            var i = 0;
        }
}
}
