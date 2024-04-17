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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectOffice.Pages
{
    /// <summary>
    /// Логика взаимодействия для OknoCal.xaml
    /// </summary>
    public partial class OknoCal : Page
    {
        public OknoCal()
        {
            InitializeComponent();
            ComboPoisk.ItemsSource = new string[] { "1 неделя", "2 недели", "1 месяц", "1 год" };
            ComboPoisk.SelectedIndex = 0;

            Refresh();
        }

        private void Refresh()
        {
            DataDate.ColumnDefinitions.Clear();
            DataDate.RowDefinitions.Clear();

            DateTime DateStart = DateTime.Now;
            DateTime DateEnd = DateTime.Now;
            int delta = 1;

            if (ComboPoisk.SelectedIndex == 0)
            {
                DateStart = DateTime.Now;
                DateEnd = DateTime.Now.AddDays(7);
                delta = 1;
            }
            if (ComboPoisk.SelectedIndex == 1)
            {
                DateStart = DateTime.Now;
                DateEnd = DateTime.Now.AddDays(14);
                delta = 2;
            }
            if (ComboPoisk.SelectedIndex == 2)
            {
                DateStart = DateTime.Now;
                DateEnd = DateTime.Now.AddMonths(1);
                delta = 4;
            }
            if (ComboPoisk.SelectedIndex == 3)
            {
                DateStart = DateTime.Now;
                DateEnd = DateTime.Now.AddYears(1);
                delta = 48;
            }

            delta = delta * (Int32.Parse(MachText.Text));

            TimeSpan timeSpan = TimeSpan.Zero;
            while (timeSpan.TotalHours != 24)
            {
                timeSpan = timeSpan + TimeSpan.FromHours(1);
                DataDate.RowDefinitions.Add(new RowDefinition());
            }

            DateTime dateTime = DateStart;
            while (dateTime < DateEnd)
            {
                dateTime = dateTime.AddDays(delta);
                DataDate.ColumnDefinitions.Add(new ColumnDefinition());
                for (int i = 0; i < 23; i++)
                {
                    TextBlock textBlock = new TextBlock() { Text = "dag"};
                    DataDate.Children.Add(textBlock);
                    Grid.SetColumn(textBlock, DataDate.Children.Count);
                    Grid.SetRow(textBlock, i);
                }
            }

            DateText.Text = DateStart.Day + "." + DateStart.Month + " - " + DateEnd.Day + "." + DateEnd.Month;
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) > 1)
            {
                MachText.Text = (Int32.Parse(MachText.Text) - 1).ToString();
                Refresh();
            }
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) < 10)
            {
                MachText.Text = (Int32.Parse(MachText.Text) + 1).ToString();
                Refresh();
            }
        }

        private void DownDateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpDateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboPoisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OknoDash());
        }
    }
}
