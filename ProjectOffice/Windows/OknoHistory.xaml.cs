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
    /// Логика взаимодействия для OknoHistory.xaml
    /// </summary>
    public partial class OknoHistory : Window
    {
        public OknoHistory(Models.Task task)
        {
            InitializeComponent();
            DataHistory.ItemsSource = App.DB.LastStatus.Where(x => x.TaskId == task.Id).OrderBy(x => x.Date).ToList();
        }
    }
}
