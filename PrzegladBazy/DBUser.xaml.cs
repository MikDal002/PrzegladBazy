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

namespace PrzegladBazy
{
    /// <summary>
    /// Logika interakcji dla klasy DBUser.xaml
    /// </summary>
    public partial class DBUser : Window
    {
        private MainWindow _mainWindow;
        public DBUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeDatabaseConnection(server.Text, database.Text, user.Text, password.Password);
            this.Close();
        }
    }
}
