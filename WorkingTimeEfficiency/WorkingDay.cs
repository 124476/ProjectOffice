using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingTimeEfficiency
{
    public class WorkingDay
    {
        public DateTime Date { get; set; }
        private int hours;
        public int Hours
        {
            get => (Status == StatusDay.Working || Status == StatusDay.PreWorking) ? hours : 0;
            set => hours = (Status == StatusDay.Working || Status == StatusDay.PreWorking) ? value : 0;
        }
        public StatusDay Status { get; set; }
    }

    public enum StatusDay
    {
        Working,            // Рабочий день
        PreWorking,         // Предпраздничный день
        SickLeave,          // Больничный
        DisrespectfulSash,  // Отсутвие по неуважительной причине
        Holiday             // Отпуск или выходной нерабочий день
    }

}
