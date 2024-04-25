using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingTimeEfficiency
{
    public class Task
    {
        public string Guid { get; set; }
        public string FullTitle { get; set; }
        public string Description { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
