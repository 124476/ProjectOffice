using Microsoft.Win32;
using ProjectOffice.Models;
using ProjectOffice.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для OknoTask.xaml
    /// </summary>
    public partial class OknoTask : Page
    {
        Models.Task contextTask;
        public OknoTask()
        {
            InitializeComponent();
            ComboIsplos.ItemsSource = App.DB.Employe.ToList();
            ComboIsplosies.ItemsSource = App.DB.Employe.ToList();
            DataContext = contextTask;
        }

        private void PoiskText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            var searchText = PoiskText.Text;
            var filtred = App.DB.Task.Where(x => x.ProjectId == App.project.Id && x.IsDelete != true).ToList();

            if (string.IsNullOrWhiteSpace(searchText) != true)
                filtred = filtred.Where(x => x.Name.Contains(PoiskText.Text) || x.Descriprion.Contains(PoiskText.Text)).ToList();
            
            filtred = filtred.Where(x => x.StatusId != 5).ToList();

            filtred = filtred.OrderByDescending(x => x.StatusId == 3 && x.DateEnd?.Date < DateTime.Now.Date)
                             .ThenByDescending(x => x.StatusId == 2 && x.DateEnd?.Date < DateTime.Now.Date)
                             .ThenByDescending(x => x.StatusId == 3)
                             .ThenByDescending(x => x.StatusId == 2).ToList();

            ListTasks.ItemsSource = filtred;

            if (contextTask != null)
            {
                ListWathers.ItemsSource = App.DB.Wather.Where(x => x.TaskId == contextTask.Id).Select(x => x.Employe).ToList();
                ListPhotos.ItemsSource = App.DB.DocumentFile.Where(x => x.TaskId == contextTask.Id).ToList();
                var users = App.DB.Employe.ToList();
                users.Insert(0, new Employe() { First_name="Добавить" });
                ComboWathes.ItemsSource = users;
                ComboWathes.SelectedIndex = 0;
            }
        }

        private void ListTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Task task = ListTasks.SelectedItem as Models.Task;
            if (task != null)
            {
                TaskColumn.Width = new GridLength(1, GridUnitType.Star);
                TaskColumnNew.Width = new GridLength(0);
                contextTask = task;
                ComboIsplos.ItemsSource = App.DB.Employe.ToList();
                DataContext = contextTask;
                ComboIsplos.SelectedItem = contextTask.Ispol;
                ComboTasks.SelectedItem = contextTask.LastTask;
                Refresh();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            TaskColumnNew.Width = new GridLength(1, GridUnitType.Star);
            TaskColumn.Width = new GridLength(0);

            contextTask = new Models.Task();
            contextTask.ProjectId = App.project.Id;
            ComboIsplos.ItemsSource = App.DB.Employe.ToList();
            ComboTasks.ItemsSource = App.DB.Task.Where(x => x.ProjectId == App.project.Id && x.IsDelete != true).ToList();
            DataContext = contextTask;
            Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var task = App.DB.Task.FirstOrDefault(x => x.Id == contextTask.PreviousTaskId);
            var tasks = App.DB.Task.Where(x => x.PreviousTaskId == contextTask.Id);
            foreach(var item in tasks)
            {
                item.PreviousTaskId = task.Id;
            }

            contextTask.IsDelete = true;
            App.DB.SaveChanges();
            TaskColumnNew.Width = new GridLength(0);
            TaskColumn.Width = new GridLength(0);
            Refresh();
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageHistory(contextTask));
        }

        private void ComboWathes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employe employe = ComboWathes.SelectedItem as Employe;
            if (ComboWathes.SelectedIndex != 0 && employe != null)
            {
                Wather wather = new Wather();
                wather.Employe = employe;
                wather.Task = contextTask;
                App.DB.Wather.Add(wather);
                App.DB.SaveChanges();
                Refresh();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (contextTask.Id == 0)
            {
                if (FinishActualTime.SelectedDate != null && CreatedTime.SelectedDate != null && ComboIsplos.SelectedIndex != -1
                    && contextTask.ShortTitle != null && contextTask.Name != null)
                {
                    Employe employe = ComboIsplos.SelectedItem as Employe;
                    contextTask.IspolnitelId = employe.Id;
                    contextTask.StatusId = 2;

                    Models.Task task = ComboTasks.SelectedItem as Models.Task;
                    if (task != null)
                    {
                        contextTask.PreviousTaskId = task.Id;
                    }
                    App.DB.Task.Add(contextTask);
                    App.DB.SaveChanges();
                    TaskColumnNew.Width = new GridLength(0);
                    TaskColumn.Width = new GridLength(0);
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }
            }
            else
            {
                Employe employe = ComboIsplos.SelectedItem as Employe;
                contextTask.IspolnitelId = employe.Id;
                App.DB.SaveChanges();
                TaskColumnNew.Width = new GridLength(0);
                TaskColumn.Width = new GridLength(0);
            }
            Refresh();
        }

        private void DontBtn_Click(object sender, RoutedEventArgs e)
        {
            TaskColumnNew.Width = new GridLength(0);
            TaskColumn.Width = new GridLength(0);
        }

        private void PhotoGot_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = "*.png; | *.png;" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                DocumentFile documentFile = new DocumentFile();
                documentFile.Photo = File.ReadAllBytes(dialog.FileName);
                documentFile.Name = dialog.FileName;
                documentFile.Task = contextTask;
                App.DB.DocumentFile.Add(documentFile);
                App.DB.SaveChanges();
                Refresh();
            }
        }
    }
}
