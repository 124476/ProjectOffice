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
    /// Логика взаимодействия для OknoTask.xaml
    /// </summary>
    public partial class OknoTask : Page
    {
        public OknoTask()
        {
            InitializeComponent();
            ListTasks.ItemsSource = App.DB.Task.Where(x => x.ProjectId == App.project.Id).OrderBy(x => x.StatusId + x.FinishActualTime.ToString()).ToList();
        }

        private void PoiskText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refrash();
        }

        private void Refrash()
        {
            ListTasks.ItemsSource = App.DB.Task.Where(x => x.ProjectId == App.project.Id).Where(x => x.Name.Contains(PoiskText.Text) || x.Descriprion.Contains(PoiskText.Text)).OrderBy(x => x.StatusId + x.FinishActualTime.ToString()).ToList();
        }
    }
}
