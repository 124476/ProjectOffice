using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
