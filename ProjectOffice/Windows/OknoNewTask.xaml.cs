using ProjectOffice.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для OknoNewTask.xaml
    /// </summary>
    public partial class OknoNewTask : Window
    {
        Models.Task contextTask;
        public OknoNewTask()
        {
            InitializeComponent();
            contextTask = new Models.Task();
            contextTask.ProjectId = App.project.Id;
            ComboIsplos.ItemsSource = App.DB.Employe.ToList();
            ComboTasks.ItemsSource = App.DB.Task.ToList();
            DataContext = contextTask;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }
    }
}
