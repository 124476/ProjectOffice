﻿using Microsoft.Win32;
using ProjectOffice.Properties;
using ProjectOffice.Windows;
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

            Refresh(false, null);
        }

        private void Refresh(bool isCan, Models.Task gotTask)
        {
            List<Models.Task> listTasks = null;

            if (gotTask != null)
            {
                listTasks = new List<Models.Task>();
                listTasks.Add(gotTask);
                while (true)
                {
                    var listId = listTasks[listTasks.Count() - 1];
                    if (listId != null)
                    {
                        listTasks.Add(App.DB.Task.FirstOrDefault(x => x.Id == listId.PreviousTaskId));
                    }
                    else
                    {
                        break;
                    }
                }

               while (true)
                {
                    var listId = listTasks[listTasks.Count() - 1];
                    if (listId != null && App.DB.Task.FirstOrDefault(x => x.PreviousTaskId == listId.Id) != null)
                    {
                        listTasks.Add(App.DB.Task.FirstOrDefault(x => x.PreviousTaskId == listTasks[listTasks.Count - 1].Id));
                    }
                    else
                    {
                        break;
                    }
                }
            }

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
                        if (item.DateStart.Value.AddHours(-item.DateStart.Value.Hour).AddMinutes(-item.DateStart.Value.Minute).AddSeconds(-item.DateStart.Value.Second) <= dateTime && item.DateEnd == null)
                        {
                            taskDays.Add(item);
                            continue;
                        }
                        if (item.DateStart.Value.AddHours(-item.DateStart.Value.Hour).AddMinutes(-item.DateStart.Value.Minute).AddSeconds(-item.DateStart.Value.Second) <= dateTime && item.DateEnd >= dateTime)
                        {
                            taskDays.Add(item);
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
                        if (taskDays[i].DateStart.Value.AddMinutes(-taskDays[i].DateStart.Value.Minute).AddSeconds(-taskDays[i].DateStart.Value.Second) <= dateTime.AddHours(timeDelta.Hours) && taskDays[i].DateEnd >= dateTime.AddHours(timeDelta.Hours))
                        {
                            string colorHex = "";

                            if (isCan && listTasks != null && listTasks.Contains(taskDays[i]))
                            {
                                colorHex = "#EDA0FA";
                            }
                            else
                            {
                                colorHex = "#EDF0FF";
                            }

                            if ((Int32)dateTime.DayOfWeek == 6 || (Int32)dateTime.DayOfWeek == 0)
                            {
                                if (isCan && listTasks.Contains(taskDays[i]))
                                {
                                    colorHex = "#AAE2E2";
                                }
                                else
                                {
                                    colorHex = "#FFE2E2";
                                }
                            }

                            if (listProv.Contains(taskDays[i]) == false)
                            {
                                listProv.Add(taskDays[i]);
                                textBlock = new TextBlock()
                                {
                                    Text = taskDays[i].Name,
                                    TextWrapping = TextWrapping.Wrap,
                                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex)),
                                    DataContext = taskDays[i]
                                };
                            }
                            else
                            {
                                textBlock = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex)), DataContext = taskDays[i] };
                            }
                            textBlock.MouseUp += new MouseButtonEventHandler(TextBlock_Cliek);
                            textBlock.MouseEnter += TextBlock_MouseEnter;


                            if (taskDays[i].DateEnd >= DateEnd || taskDays[i].DateStart <= DateStart)
                            {
                                Grid grid = new Grid() { };
                                grid.Children.Add(textBlock);

                                if (taskDays[i].DateStart <= DateStart && taskDays[i].DateEnd >= DateEnd)
                                {
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                                    TextBlock textColor = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")), DataContext = taskDays[i] };
                                    TextBlock textColor2 = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")), DataContext = taskDays[i] };
                                    grid.Children.Add(textColor);
                                    grid.Children.Add(textColor2);
                                    Grid.SetColumn(textColor, 0);
                                    Grid.SetColumn(textBlock, 1);
                                    Grid.SetColumn(textColor2, 2);
                                }

                                if (taskDays[i].DateStart <= DateStart && taskDays[i].DateEnd < DateEnd)
                                {
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                    TextBlock textColor = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")), DataContext = taskDays[i] };
                                    grid.Children.Add(textColor);
                                    Grid.SetColumn(textBlock, 1);
                                    Grid.SetColumn(textColor, 0);
                                }

                                if (taskDays[i].DateStart > DateStart && taskDays[i].DateEnd >= DateEnd)
                                {
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                                    TextBlock textColor = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75F0")), DataContext = taskDays[i] };
                                    grid.Children.Add(textColor);
                                    Grid.SetColumn(textBlock, 0);
                                    Grid.SetColumn(textColor, 1);
                                }
                                gridOne.Children.Add(grid);
                                Grid.SetColumn(grid, i);
                                }
                                else
                                {
                                    gridOne.Children.Add(textBlock);
                                    Grid.SetColumn(textBlock, i);
                                }
                            }
                            else
                            {
                                textBlock = new TextBlock() { DataContext = taskDays[i] };
                            }
                        }
                        DataDate.Children.Add(gridOne);
                        Grid.SetColumn(gridOne, DataDate.ColumnDefinitions.Count() - 1);
                        Grid.SetRow(gridOne, timeDelta.Hours);
                        timeDelta = timeDelta + TimeSpan.FromHours(1);
                    }

                    TextBlock textDate = new TextBlock()
                    {
                        Text = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTime.DayOfWeek) + " " + dateTime.ToLongDateString(),
                        TextWrapping = TextWrapping.Wrap,
                        Background = Brushes.White
                    };
                    DataDate.Children.Add(textDate);
                    Grid.SetRow(textDate, 24);
                    Grid.SetColumn(textDate, DataDate.ColumnDefinitions.Count() - 1);
                    dateTime = dateTime.AddDays(1);
                }

                DateText.Text = DateStart.Day + "." + DateStart.Month + "." + DateStart.Year + " - " + DateEnd.Day + "." + DateEnd.Month + "." + DateEnd.Year;
            }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Models.Task task = (sender as TextBlock).DataContext as Models.Task;
            Refresh(true, task);
        }

        private void TextBlock_Cliek(object sender, MouseButtonEventArgs e)
        {
            Models.Task task = (sender as TextBlock).DataContext as Models.Task;
            var dialog = new OknoInfoTask(task);
            dialog.ShowDialog();
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) > 1)
            {
                MachText.Text = (Int32.Parse(MachText.Text) - 1).ToString();
                Settings.Default.delta = Int32.Parse(MachText.Text);
                Settings.Default.Save();
                Refresh(false, null);
            }
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(MachText.Text) < 10)
            {
                MachText.Text = (Int32.Parse(MachText.Text) + 1).ToString();
                Settings.Default.delta = Int32.Parse(MachText.Text);
                Settings.Default.Save();
                Refresh(false, null);
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

            Refresh(false, null);
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
            Refresh(false, null);
        }

        private void ComboPoisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.dateDelta = (Int32)ComboPoisk.SelectedIndex;
            Settings.Default.Save();
            Refresh(false, null);
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter= "*.xlsx; | *.xlsx;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                MessageBox.Show("Import");
            }

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OknoDash());
        }   
    }
}