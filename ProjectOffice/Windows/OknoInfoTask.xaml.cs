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
            DateTime dateTime = DateTime.Parse((task.DateEnd.Value - task.DateStart.Value).ToString());
            DateTime dateNew = DateTime.Parse("00-00-00");

            if (dateTime.Year != 0)
            {
                if (dateTime.Month != 0)
                {
                    if (dateTime.Month > 6)
                    {
                        dateNew = dateNew.AddYears(dateTime.Year + 1);
                    }
                    else
                    {
                        dateNew = dateNew.AddYears(dateTime.Year);
                    }
                }
                else
                {
                    dateNew = dateNew.AddYears(dateTime.Year);
                }
            }
            else
            {
                if (dateTime.Month != 0)
                {
                    if (dateTime.Day != 0)
                    {
                        if (dateTime.Day > 15)
                        {
                            dateNew = dateNew.AddMonths(dateTime.Month + 1);
                        }
                        else
                        {
                            dateNew = dateNew.AddMonths(dateTime.Month);
                        }
                    }
                    else
                    {
                        dateNew = dateNew.AddMonths(dateTime.Month);
                    }
                }
                else
                {
                    if (dateTime.Day != 0)
                    {
                        if (dateTime.Hour != 0)
                        {
                            if (dateTime.Hour > 12)
                            {
                                dateNew = dateNew.AddDays(dateTime.Hour + 1);
                            }
                            else
                            {
                                dateNew = dateNew.AddDays(dateTime.Hour);
                            }
                        }
                        else
                        {
                            dateNew = dateNew.AddDays(dateTime.Hour);
                        }
                    }
                    else
                    {
                        if (dateTime.Hour != 0)
                        {
                            if (dateTime.Minute != 0)
                            {
                                if (dateTime.Minute > 30)
                                {
                                    dateNew = dateNew.AddHours(dateTime.Minute + 1);
                                }
                                else
                                {
                                    dateNew = dateNew.AddHours(dateTime.Minute);
                                }
                            }
                            else
                            {
                                dateNew = dateNew.AddHours(dateTime.Minute);
                            }
                        }
                        else
                        {
                            if (dateTime.Minute != 0)
                            {
                                if (dateTime.Second != 0)
                                {
                                    if (dateTime.Second > 30)
                                    {
                                        dateNew = dateNew.AddHours(dateTime.Second + 1);
                                    }
                                    else
                                    {
                                        dateNew = dateNew.AddHours(dateTime.Second);
                                    }
                                }
                                else
                                {
                                    dateNew = dateNew.AddHours(dateTime.Second);
                                }
                            }
                        }
                    }
                }
            }
            string dateNewText = "";
            if (dateNew.Year != 0)
            {
                dateNewText = dateNew.Year.ToString() + " " + "years";
            }
            if (dateNew.Month != 0)
            {
                dateNewText += dateNew.Month.ToString() + " " + "months";
            }
            if (dateNew.Day != 0)
            {
                dateNewText += dateNew.Day.ToString() + " " + "days";
            }
            if (dateNew.Hour != 0)
            {
                dateNewText += dateNew.Hour.ToString() + " " + "hours";
            }
            if (dateNew.Minute != 0)
            {
                dateNewText += dateNew.Minute.ToString() + " " + "minutes";
            }
            if (dateNew.Second != 0)
            {
                dateNewText += dateNew.Second.ToString() + " " + "seconds";
            }

            InfoText.Text = dateNewText;
        }
    }
}
