using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectOffice.Models
{
    public partial class Task
    {
        public string EmployeeIspolName
        {
            get
            {
                Employe employe = App.DB.Employe.FirstOrDefault(x => x.Id == IspolnitelId);
                return employe.First_name + " " + employe.Last_name[0] + ". " + employe.Middle_name[0] + ".";
            }
        }

        public string BackColor
        {
            get
            {
                if (FinishActualTime < DateTime.Now)
                {
                    return "Red";
                }
                return "Gray";
            }
        }

        public string LastTask
        {
            get
            {
                Models.Task task = App.DB.Task.FirstOrDefault(x => x.Id == PreviousTaskId);
                if (task != null)
                {
                    return task.ShortTitle;
                }
                else
                {
                    return null;
                }
            }
        }

        public Employe Ispol
        {
            get
            {
                return App.DB.Employe.FirstOrDefault(x => x.Id == IspolnitelId);
            }
            set
            {
                value = App.DB.Employe.FirstOrDefault(x => x.Id == IspolnitelId);
            }
        }
    }
}
