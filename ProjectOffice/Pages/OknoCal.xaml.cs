using ProjectOffice.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
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
        DateTime DateStart;
        DateTime DateEnd;

        public OknoCal()
        {
            InitializeComponent();
            ComboPoisk.ItemsSource = new string[] { "1 неделя", "2 недели", "1 месяц", "1 год" };
            ComboPoisk.SelectedIndex = Settings.Default.dateDelta;

            if (Settings.Default.DateStart != null)
            {
                DateStart = Settings.Default.DateStart;
            }
            else
            {
                DateStart = DateTime.Now;
            }
            MachText.Text = Settings.Default.delta.ToString();

            Refresh();
        }

        private void Refresh()
        {
            DataDate.Children.Clear();
            DataDate.ColumnDefinitions.Clear();
            DataDate.RowDefinitions.Clear();

            if (ComboPoisk.SelectedIndex == 0)
            {
                DateEnd = DateStart.AddDays(6);
            }
            if (ComboPoisk.SelectedIndex == 1)
            {
                DateEnd = DateStart.AddDays(13);
            }
            if (ComboPoisk.SelectedIndex == 2)
            {
                DateEnd = DateStart.AddMonths(1).AddDays(-1);
            }
            if (ComboPoisk.SelectedIndex == 3)
            {
                DateEnd = DateStart.AddYears(1).AddDays(-1);
            }

            TimeSpan timeSpan = TimeSpan.Zero;
            while (timeSpan.TotalHours != 24)
            {
                TextBlock textBlock = new TextBlock() { Text = timeSpan.ToString() };
                timeSpan = timeSpan + TimeSpan.FromHours(1);
                DataDate.RowDefinitions.Add(new RowDefinition());
                DataDate.Children.Add(textBlock);
                Grid.SetRow(textBlock, DataDate.RowDefinitions.Count() - 1);
            }
            DataDate.RowDefinitions.Add(new RowDefinition());

            var tasks = App.DB.Task.Where(x => x.ProjectId == App.project.Id).ToList();

            DateTime dateTime = DateStart;
            while (dateTime <= DateEnd)
            {
                if (DataDate.ColumnDefinitions.Count() == 0)
                {
                    DataDate.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                    continue;
                }
                DataDate.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30 * Settings.Default.delta) });

                List<Models.Task> taskDays = new List<Models.Task>();
                try
                {
                    foreach (var item in tasks)
                    {
                        if (item.DateStart.Value.AddHours(-item.DateStart.Value.Hour) <= dateTime && item.DateEnd >= dateTime)
                        {
                            taskDays.Add(item);
                            continue;
                        }
                    }
                }
                catch
                {

                }
                List<Models.Task> listProv = new List<Models.Task>();
                TimeSpan timeDelta = TimeSpan.Parse("00:00:00");
                while (timeDelta <= TimeSpan.Parse("23:00:00"))
                {
                    Grid gridOne = new Grid();
                    foreach (var task in taskDays)
                    {
                        gridOne.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    for (int i = 0; i < taskDays.Count(); i++)
                    {
                        TextBlock textBlock = new TextBlock() { };
                        if (taskDays[i].DateStart <= dateTime.AddHours(timeDelta.Hours) && taskDays[i].DateEnd >= dateTime.AddHours(timeDelta.Hours))
                        {
                            string colorHex = "#EDF0FF";
                            if ((Int32)dateTime.DayOfWeek == 6 || (Int32)dateTime.DayOfWeek == 0)
                            {
                                colorHex = "#FFE2E2";
                            }

                            if (listProv.Contains(taskDays[i]) == false)
                            {
                                listProv.Add(taskDays[i]);
                                textBlock = new TextBlock() { Text = taskDays[i].Name, TextWrapping = TextWrapping.Wrap,
                                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex))
                                };
                            }
                            else
                            {
                                textBlock = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex)) };
                            }

                            StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                            if (taskDays[i].DateEnd >= DateEnd || taskDays[i].DateStart <= DateStart)
                            {
                                if (taskDays[i].DateEnd >= DateEnd)
                                {
                                    TextBlock textColor = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")) };
                                    //stackPanel.Children.Add(textBlock);
                                    //stackPanel.Children.Add(textColor);
                                    gridOne.Children.Add(textBlock);
                                    Grid.SetColumn(textBlock, i);
                                }

                                if (taskDays[i].DateStart <= DateStart)
                                {
                                    TextBlock textColor = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")) };
                                    //stackPanel.Children.Add(textColor);
                                    //stackPanel.Children.Add(textBlock);
                                    gridOne.Children.Add(textBlock);
                                    Grid.SetColumn(textBlock, i);
                                }
                            }
                            else
                            {
                                gridOne.Children.Add(textBlock);
                                Grid.SetColumn(textBlock, i);
                            }
                        }
                        else
                        {
                            textBlock = new TextBlock() {};
                        }
                    }
                    DataDate.Children.Add(gridOne);
                    Grid.SetColumn(gridOne, DataDate.ColumnDefinitions.Count() - 1);
                    Grid.SetRow(gridOne, timeDelta.Hours);
                    timeDelta = timeDelta + TimeSpan.FromHours(1);
                }

                TextBlock textDate = new TextBlock() { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTime.DayOfWeek) + " " + dateTime.ToLongDateString(), 
                                                        TextWrapping=TextWrapping.Wrap, Background=Brushes.White};
                DataDate.Children.Add(textDate);
                Grid.SetRow(textDate, 24);
                Grid.SetColumn(textDate, DataDate.ColumnDefinitions.Count() - 1);
                dateTime = dateTime.AddDays(1);
            }



            DateText.Text = DateStart.Day + "." + DateStart.Month + "." + DateStart.Year + " - " + DateEnd.Day + "." + DateEnd.Month + "." + DateEnd.Year;
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) > 1)
            {
                MachText.Text = (Int32.Parse(MachText.Text) - 1).ToString();
                Settings.Default.delta = Int32.Parse(MachText.Text);
                Settings.Default.Save();
                Refresh();
            }
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) < 10)
            {
                MachText.Text = (Int32.Parse(MachText.Text) + 1).ToString();
                Settings.Default.delta = Int32.Parse(MachText.Text);
                Settings.Default.Save();
                Refresh();
            }
        }

        private void DownDateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboPoisk.SelectedIndex == 0)
            {
                DateStart = DateStart.AddDays(-7);
            }
            if (ComboPoisk.SelectedIndex == 1)
            {
                DateStart = DateStart.AddDays(-14);
            }
            if (ComboPoisk.SelectedIndex == 2)
            {
                DateStart = DateStart.AddMonths(-1);
            }
            if (ComboPoisk.SelectedIndex == 3)
            {
                DateStart = DateStart.AddYears(-1);
            }

            Settings.Default.DateStart = DateStart;
            Settings.Default.Save();

            Refresh();
        }

        private void UpDateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboPoisk.SelectedIndex == 0)
            {
                DateStart = DateStart.AddDays(7);
            }
            if (ComboPoisk.SelectedIndex == 1)
            {
                DateStart = DateStart.AddDays(14);
            }
            if (ComboPoisk.SelectedIndex == 2)
            {
                DateStart = DateStart.AddMonths(1);
            }
            if (ComboPoisk.SelectedIndex == 3)
            {
                DateStart = DateStart.AddYears(1);
            }
            Settings.Default.DateStart = DateStart;
            Settings.Default.Save();
            Refresh();
        }

        private void ComboPoisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.dateDelta = (Int32)ComboPoisk.SelectedIndex;
            Settings.Default.Save();
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
