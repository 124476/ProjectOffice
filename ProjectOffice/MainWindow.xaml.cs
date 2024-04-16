using ProjectOffice.Models;
using ProjectOffice.Pages;
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

namespace ProjectOffice
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.project = App.DB.Project.FirstOrDefault();
            MyFrame.Navigate(new OknoTask());
            ListProjects.ItemsSource = App.DB.Project.ToList();
        }

        private void DashBtn_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new OknoDash());
        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new OknoTask());
        }

        private void CalBtn_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new OknoCal());
        }

        private void ProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            Project project = (sender as Button).DataContext as Project;
            App.project = project;
            ListProjects.ItemsSource = App.DB.Project.ToList();
        }
    }
}
