using ProjectOffice.Models;
using ProjectOffice.Windows;
using System;
using System.Collections.Generic;
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
        public OknoTask()
        {
            InitializeComponent();
            Refresh();
        }

        private void PoiskText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            var searchText = PoiskText.Text;
            var filtred = App.DB.Task.Where(x => x.ProjectId == App.project.Id).ToList();

            if (string.IsNullOrWhiteSpace(searchText) != true)
                filtred = filtred.Where(x => x.Name.Contains(PoiskText.Text) || x.Descriprion.Contains(PoiskText.Text)).ToList();
            
            filtred = filtred.Where(x => x.StatusId != 5).ToList();

            filtred = filtred.OrderByDescending(x => x.StatusId == 3 && x.DateEnd?.Date < DateTime.Now.Date)
                             .ThenByDescending(x => x.StatusId == 2 && x.DateEnd?.Date < DateTime.Now.Date)
                             .ThenByDescending(x => x.StatusId == 3)
                             .ThenByDescending(x => x.StatusId == 2).ToList();

            ListTasks.ItemsSource = filtred;
        }

        private void ListTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Task task = ListTasks.SelectedItem as Models.Task;
            if (task != null)
            {
                var dialog = new OknoOneTask(task);
                dialog.ShowDialog();
                if (dialog.DialogResult == true)
                {
                    Refresh();
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OknoNewTask();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                Refresh();
            }
        }
    }
}
