using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ProjectOffice.Models
{
    public partial class Task
    {
        [XmlIgnore]
        public string EmployeeIspolName
        {
            get
            {
                Employe employe = App.DB.Employe.FirstOrDefault(x => x.Id == IspolnitelId);
                return employe.First_name + " " + employe.Last_name[0] + ". " + employe.Middle_name[0] + ".";
            }
        }
        [XmlIgnore]

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

        [XmlIgnore]
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
        [XmlIgnore]

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
