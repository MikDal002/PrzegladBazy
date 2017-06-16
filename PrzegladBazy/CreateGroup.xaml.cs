using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;
using PrzegladBazy.Models;

namespace PrzegladBazy
{
    /// <summary>
    /// Logika interakcji dla klasy CreateGroup.xaml
    /// </summary>
    public partial class CreateGroup
    {
        /// <summary>
        /// Dostęp do danych z bazy
        /// </summary>
        private readonly wizualizacja2Entities1 _context = new wizualizacja2Entities1();
        /// <summary>
        /// Okno główne programu. 
        /// Generalnie ten wskaźnik należy usunąć, bo możliwe jest wykonanie tego połączenia
        /// bez bezpośredniego połączenia z oknem głównym.
        /// </summary>
        private readonly MainWindow _mainWindow;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="mainWindow">Uchwyt do okna głównego aplikacji</param>
        public CreateGroup(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            try
            {
                if (!string.IsNullOrEmpty(mainWindow.ConnectionString))
                    _context.Database.Connection.ConnectionString = mainWindow.ConnectionString;
                
                foreach (var tocheck in _context.Slownik)
                {
                    LvToCheck.Items.Add(new CheckBox { Content = tocheck.LongGate, IsChecked = false });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            
        }
        /// <summary>
        /// Odpowiedź na naciśnięcie przycisku "do prawej".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ToRight_Click(object sender, RoutedEventArgs e)
        {
            // Odczytaj zaznaczone elementy z lewego okna
            var listaTmp = (from object tochec in LvToCheck.Items select tochec as CheckBox into toCheck where !(toCheck is null) where toCheck.IsChecked == true select toCheck).ToList();

            // Przesuń elementy do prawej listy.
            foreach (var elem in listaTmp)
            {
                elem.IsChecked = false;
                LvToCheck.Items.Remove(elem);
                LvChecked.Items.Add(elem);
            }
            
        }
        /// <summary>
        /// Przycisk "do lewej".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ToLeft_CLick(object sender, RoutedEventArgs e)
        {
            // Odczytaj zaznaczone elementy z prawej listy
            var listaTmp = (from object tochec in LvChecked.Items select tochec as CheckBox into toCheck where !(toCheck is null) where toCheck.IsChecked == true select toCheck).ToList();

            // Przesuń elementy do listy po lewej
            foreach (var elem in listaTmp)
            {
                elem.IsChecked = false;
                LvChecked.Items.Remove(elem);
                LvToCheck.Items.Add(elem);
            }
        }
        /// <summary>
        /// Przycisk "zapisz" grupę.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            var grupa = new List<string>();

            // Pobierz elementy z prawej listy
            foreach (var item in LvChecked.Items)
            {
                var chb = item as CheckBox;
                if (chb == null)
                    continue;
                
                grupa.Add(chb.Content.ToString());
            }

            // Utwórz nową grupę słowników
            var slgrp = new SlownikGroup
            {
                Title = TbGroupName.Text,
                Slowniki = grupa
            };

            // Zapisz nową grupę do pliku
            // TODO: Poniższa ścieżka powinna być stałą!
            XmlSerializer xs = new XmlSerializer(typeof(SlownikGroup));
            var sciezka = System.IO.Path.GetFullPath(@".\Groups\" + TbGroupName.Text + @".xml");
            (new FileInfo(sciezka).Directory)?.Create();

            TextWriter tw = new StreamWriter(sciezka);
            xs.Serialize(tw, slgrp);

            // Dodaj grupę do listy w oknie głównym.
            _mainWindow.Groups.Add(slgrp);
            Close();
        }
    }
}
