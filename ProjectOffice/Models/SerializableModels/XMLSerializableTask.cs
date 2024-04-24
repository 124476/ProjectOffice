using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOffice.Models.SerializableModels
{
    public class XMLSerializableTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortTitle { get; set; }
        public string Status { get; set; }
    }
}
