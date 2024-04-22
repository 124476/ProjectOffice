using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOffice.Models
{
    public partial class Employe
    {
        public string FullName
        {
            get
            {
                return First_name + " " + Last_name + " " + Middle_name;
            }
        }
        public int KolWin
        {
            get
            {
                var dateNow = DateTime.Now;
                var datePoisk = dateNow.AddMonths(-1);
                int kol = App.DB.Task.Where(x => x.ProjectId == App.project.Id && dateNow >= x.DateEnd && datePoisk <= x.DateEnd && Id == x.IspolnitelId).Count();
                return kol;
            }
        }
        public int KolLosed
        {
            get
            {
                var dateNow = DateTime.Now;
                var datePoisk = dateNow.AddMonths(-1);
                int kol = App.DB.Task.Where(x => x.ProjectId == App.project.Id && x.DateEnd == null 
                && x.FinishActualTime <= dateNow && x.FinishActualTime >= datePoisk && Id == x.IspolnitelId).Count();
                return kol;
            }
        }
    }
}
