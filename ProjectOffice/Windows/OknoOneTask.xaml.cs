using ProjectOffice.Models;
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

namespace ProjectOffice.Windows
{
    /// <summary>
    /// Логика взаимодействия для OknoOneTask.xaml
    /// </summary>
    public partial class OknoOneTask : Window
    {
        Models.Task contextTask;
        public OknoOneTask(Models.Task task)
        {
            InitializeComponent();
            contextTask = task;
            ComboIsplos.ItemsSource = App.DB.Employe.ToList();
            DataContext = contextTask;
            ComboIsplos.SelectedItem = contextTask.Ispol;
            Refrash();
        }

        private void ComboWathes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employe employe = ComboWathes.SelectedItem as Employe;
            if (employe != null)
            {
                Wather wather = new Wather();
                wather.Employe = employe;
                wather.Task = contextTask;
                App.DB.Wather.Add(wather);
                App.DB.SaveChanges();
                Refrash();
            }
        }

        private void Refrash()
        {
            ListWathers.ItemsSource = App.DB.Wather.Where(x => x.TaskId == contextTask.Id).Select(x => x.Employe).ToList();
            ComboWathes.ItemsSource = App.DB.Employe.ToList();
            ComboWathes.SelectedIndex = -1;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Employe employe = ComboIsplos.SelectedItem as Employe;
            contextTask.IspolnitelId = employe.Id;
            App.DB.SaveChanges();
            DialogResult = true;
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OknoDelete();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                var tasks = App.DB.Task.Where(x => x.PreviousTaskId == contextTask.Id).ToList();
                foreach (var item in tasks)
                {
                    if (item.Id == 1)
                    {
                        continue;
                    }
                    item.PreviousTaskId -= 1;
                }
                var wathers = App.DB.Wather.Where(x => x.TaskId == contextTask.Id).ToList();
                foreach (var item in wathers)
                {
                    App.DB.Wather.Remove(item);
                }

                App.DB.Task.Remove(contextTask);
                App.DB.SaveChanges();
                DialogResult = true;
            }
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OknoHistory(contextTask);
            dialog.ShowDialog();
        }
    }
}
