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
    public partial class DbUser
    {
        /// <summary>
        /// Uchwyt do okna głównego. Ten uchwyt powinien być usunięty, bo można to zrealizować inaczej
        /// </summary>
        private readonly MainWindow _mainWindow;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mainWindow">Uchwyt do okna głównego</param>
        public DbUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        /// <summary>
        /// Reakcja na przycisk "zaloguj".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeDatabaseConnection(server.Text, database.Text, user.Text, password.Password);
            this.Close();
        }
    }
}
