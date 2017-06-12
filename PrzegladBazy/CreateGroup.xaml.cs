using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using PrzegladBazy.Models;

namespace PrzegladBazy
{
    /// <summary>
    /// Logika interakcji dla klasy CreateGroup.xaml
    /// </summary>
    public partial class CreateGroup : Window
    {
        /// <summary>
        /// Dostęp do danych z bazy
        /// </summary>
        private wizualizacja2Entities _context = new wizualizacja2Entities();
        private List<string> _grupa = new List<string>();
        private MainWindow mainWindow;

        public CreateGroup(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            foreach (var tocheck in _context.Slownik)
            {
                LvToCheck.Items.Add(new CheckBox { Content = tocheck.LongGate, IsChecked = false });
            }
        }

        private void Button_ToRight_Click(object sender, RoutedEventArgs e)
        {
            var listaTmp = (from object tochec in LvToCheck.Items select tochec as CheckBox into toCheck where !(toCheck is null) where toCheck.IsChecked == true select toCheck).ToList();

            foreach (var elem in listaTmp)
            {
                elem.IsChecked = false;
                LvToCheck.Items.Remove(elem);
                LvChecked.Items.Add(elem);
            }
            
        }

        private void Button_ToLeft_CLick(object sender, RoutedEventArgs e)
        {
            var listaTmp = (from object tochec in LvChecked.Items select tochec as CheckBox into toCheck where !(toCheck is null) where toCheck.IsChecked == true select toCheck).ToList();

            foreach (var elem in listaTmp)
            {
                elem.IsChecked = false;
                LvChecked.Items.Remove(elem);
                LvToCheck.Items.Add(elem);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
