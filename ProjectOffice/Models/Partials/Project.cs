using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectOffice.Models
{
    public partial class Project
    {
        public string ShortName
        {
            get
            {
                return FullTitle.Split(' ')[0][0].ToString() + FullTitle.Split(' ')[1][0];
            }
        }
        public string BackgroundColor
        {
            get
            {
                if (App.project.Id == Id)
                {
                    return "White";
                }
                return "Blue";
            }
        }
        public string ForengroundColor
        {
            get
            {
                if (App.project.Id == Id)
                {
                    return "Blue";
                }
                return "White";
            }
        }
    }
}
