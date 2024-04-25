using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WorkingTimeEfficiency
{
    public class WorkingTime
    {
        public static double[] CalculateEfficiency(List<WorkingDay> workingDays, List<Task> tasks)
        {
            double allTime = 0;
            double allStoping = 0;
            foreach (var item in workingDays)
            {
                allTime += item.Hours;
                if (tasks.FirstOrDefault(x => x.BeginDateTime <= item.Date && item.Date <= x.EndDateTime) != null)
                {
                    allStoping += item.Hours;
                }
            }

            allStoping = allTime - allStoping;
            var index = Math.Round((1 - (allStoping / allTime)) * 100, 2);

            return new double[] { index, allStoping * 60 * 60, allStoping * 60 * 60, 0, 0 };
        }
    }
}
