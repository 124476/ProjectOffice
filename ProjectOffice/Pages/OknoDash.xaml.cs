using Microsoft.Win32;
using Newtonsoft.Json;
using ProjectOffice.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
using System.Xml.Serialization;

namespace ProjectOffice.Pages
{
    /// <summary>
    /// Логика взаимодействия для OknoDash.xaml
    /// </summary>
    public partial class OknoDash : Page
    {
        List<Employe> employesList;
        public OknoDash()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            var dateNow = DateTime.Now;

            KolZadach.Text = App.DB.Task.Where(x => x.ProjectId == App.project.Id && dateNow.Date == x.DateEnd.Value).Count().ToString();
            DateTime datePoisk = dateNow.AddDays(7);
            KolLoseZadach.Text = App.DB.Task.Where(x => x.ProjectId == App.project.Id && x.FinishActualTime > dateNow
                                                && x.FinishActualTime < datePoisk && x.DateEnd == null).Count().ToString();
            int kolLose = App.DB.Task.Where(x => x.ProjectId == App.project.Id && x.DateEnd == null && x.FinishActualTime <= dateNow).Count();
            KolLosedZadach.Text = kolLose.ToString();

            int kolEnd = App.DB.Task.Where(x => x.ProjectId == App.project.Id && dateNow <= x.FinishActualTime && dateNow >= x.DateStart).Count();
            if (kolEnd == 0)
            {
                GridEnd.Background = Brushes.Red;
            }
            if (kolLose > 2)
            {
                GridLosed.Background = Brushes.Red;
            }

            var employes = App.DB.Task.Where(x => x.ProjectId == App.project.Id).Select(x => x.Employe).ToList();
            employesList = new List<Employe>();
            int i = 0;
            foreach (var item in employes)
            {
                if (!employesList.Contains(item))
                {
                    employesList.Add(item);
                    i++;
                }
            }

            DataUsersWin.ItemsSource = employesList.OrderByDescending(x => x.KolWin).Take(5).ToList();
            DataUsersLosed.ItemsSource = employesList.OrderByDescending(x => x.KolLosed).Take(5).ToList();

            DataGrid.ColumnDefinitions.Clear();
            DataGrid.RowDefinitions.Clear();
            DataGrid.Children.Clear();
            for (int row = 0; row < 8; row++)
            {
                DataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            }

            for (int col = 0; col < 11; col++)
            {
                if (col == 0)
                {
                    DataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
                    TextBlock mon = new TextBlock() { Text = "Mon", FontSize = 13, Background = Brushes.LightGray };
                    TextBlock web = new TextBlock() { Text = "Web", FontSize = 13, Background = Brushes.LightGray };
                    TextBlock fri = new TextBlock() { Text = "Fri", FontSize = 13, Background = Brushes.LightGray };
                    DataGrid.Children.Add(mon);
                    DataGrid.Children.Add(web);
                    DataGrid.Children.Add(fri);
                    Grid.SetRow(mon, 1);
                    Grid.SetRow(web, 3);
                    Grid.SetRow(fri, 5);
                    continue;
                }
                DataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
                TextBlock textBlock = new TextBlock() { Text = col.ToString(), Background = Brushes.LightGray };
                DataGrid.Children.Add(textBlock);
                Grid.SetColumn(textBlock, col);
            }

            dateNow = DateTime.Now;
            datePoisk = dateNow.AddDays(-70);
            DateTime datePoiskWeek;
            string colorText = "";
            var tasks = App.DB.Task.Where(x => x.ProjectId == App.project.Id && dateNow >= x.DateEnd && datePoisk <= x.DateEnd).ToList();
            for (int col = 1; col < 11; col++)
            {
                for (int row = 1; row < 8; row++)
                {
                    datePoiskWeek = dateNow.AddDays(-7 * (11 - col)).AddDays(row);
                    int sum = tasks.Where(x => datePoiskWeek.Date == x.DateEnd.Value).Count();
                    if (sum == 0)
                    {
                        colorText = "#b6bdff";
                    }
                    if (sum == 1 || sum == 2)
                    {
                        colorText = "#919cff";
                    }
                    if (sum == 3 || sum == 4 || sum == 5)
                    {
                        colorText = "#6d7cff";
                    }
                    if (sum == 6 || sum == 7 || sum == 8)
                    {
                        colorText = "#485bff";
                    }
                    if (sum >= 9)
                    {
                        colorText = "#243aff";
                    }
                    TextBlock textBlock = new TextBlock() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorText)) };
                    DataGrid.Children.Add(textBlock);
                    Grid.SetColumn(textBlock, col);
                    Grid.SetRow(textBlock, row);
                }
            }
        }

        private void AllUsers_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "*.csv; | *.csv;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var file = File.Create(dialog.FileName);
                file.Close();

                string text = "№;ФИО;Position";
                int num = 1;
                foreach (var item in employesList)
                {
                    text += "\n" + num.ToString() + ";" + item.FullName + ";" + item.Position.Name;
                    num++;
                }

                File.WriteAllText(dialog.FileName, text);
            }
        }

        private void AllTasksBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "*.json; | *.json;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var file = File.Create(dialog.FileName);
                file.Close();

                var resultJson = JsonConvert.SerializeObject(App.DB.Task.Where(x => x.ProjectId == App.project.Id).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ShortTitle,
                    Status = x.TaskStatus.Name,
                }).ToList());

                File.WriteAllText(dialog.FileName, resultJson);
            }
        }

        private void AllWinTasks_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "*.xml; | *.xml;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var file = File.Create(dialog.FileName);
                file.Close();

                var dateNow = DateTime.Now;
                var datePoisk = dateNow.AddMonths(-1);

                var tasks = App.DB.Task.Where(x => x.ProjectId == App.project.Id && dateNow >= x.DateEnd && datePoisk <= x.DateEnd).ToList();

            }
        }

        private void AllWillTasks_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog() { Filter = "*.json; | *.json;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var file = File.Create(dialog.FileName);
                file.Close();

                DateTime dateDelta = DateTime.Now.AddMonths(1);
                var resultJson = JsonConvert.SerializeObject(App.DB.Task.Where(x => x.ProjectId == App.project.Id
                && x.StatusId == 2 && x.FinishActualTime >= DateTime.Now && x.FinishActualTime <= dateDelta).Select(x => new
                {
                    x.Id,
                    x.ShortTitle,
                    x.Name,
                    Status = x.TaskStatus.Name,
                }).ToList());

                File.WriteAllText(dialog.FileName, resultJson);
            }
        }

        private void AllLastTasks_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog() { Filter = "*.txt; | *.txt;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var file = File.Create(dialog.FileName);
                file.Close();

                string text = "";
                int sum = 1;
                var dateNow = DateTime.Now;
                var datePoiskStart = dateNow.AddDays(14);
                var datePoiskEnd = dateNow.AddDays(-14);
                foreach (var item in App.DB.Task.Where(x => x.ProjectId == App.project.Id
                && (x.DateStart <= dateNow && dateNow <= x.DateEnd || x.DateStart >= dateNow && x.DateStart <= datePoiskStart || x.DateEnd <= dateNow && x.DateEnd >= datePoiskEnd)))
                {
                    text += sum.ToString() + ". " + item.Name + "\n";
                    sum++;
                }

                File.WriteAllText(dialog.FileName, text);
            }
        }
    }
}
