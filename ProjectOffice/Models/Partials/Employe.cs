using System;
using System.Collections.Generic;
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
    }
}
