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
    /// Логика взаимодействия для OknoInfoTask.xaml
    /// </summary>
    public partial class OknoInfoTask : Window
    {
        public OknoInfoTask(Models.Task task)
        {
            InitializeComponent();
            TimeSpan allTime = task.DateEnd.Value - task.DateStart.Value;
            TimeSpan dateNew;

            if (allTime.Days > 0)
            {
                dateNew = TimeSpan.FromDays(allTime.Days);
                dateNew += TimeSpan.FromHours(allTime.Hours);
                if (allTime.Minutes >= 30)
                {
                    dateNew += TimeSpan.FromHours(1);
                }
            }
            else
            {
                if (allTime.Hours > 0)
                {
                    dateNew = TimeSpan.FromHours(allTime.Hours);
                    dateNew += TimeSpan.FromMinutes(allTime.Minutes);
                    if (allTime.Seconds >= 30)
                    {
                        dateNew += TimeSpan.FromMinutes(1);
                    }
                }
                else
                {
                    dateNew = TimeSpan.FromMinutes(allTime.Minutes);
                    dateNew += TimeSpan.FromSeconds(allTime.Seconds);
                }
            }

            string allTimeString = "";

            if (dateNew.Days != 0)
                allTimeString += dateNew.Days.ToString() + " days ";
            if (dateNew.Hours != 0)
                allTimeString += dateNew.Hours.ToString() + " hours ";
            if (dateNew.Minutes != 0)
                allTimeString += dateNew.Minutes.ToString() + " minutes ";
            if (dateNew.Seconds != 0)
                allTimeString += dateNew.Seconds.ToString() + " seconds ";


            InfoText.Text = allTimeString;
        }
    }
}
