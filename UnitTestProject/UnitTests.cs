using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WorkingTimeEfficiency;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestAllDaysWorking()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var dateNow = DateTime.Now;
            var dateTime = dateNow.AddMonths(-1);
            while (dateTime != dateNow)
            {
                days.Add(new WorkingDay()
                {
                    Date = dateTime,
                    Status = StatusDay.Working,
                    Hours = 8
                });

                dateTime = dateTime.AddDays(1);
            }

            var tasks = new List<Task>();
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-21),
                EndDateTime = dateNow.AddDays(-2)
            });

            var excepted = new double[] { 64.52, 316800, 316800, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }

        [TestMethod]
        public void TestAllWorkingDaysTasks()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var dateNow = DateTime.Now;
            var dateTime = dateNow.AddMonths(-1);
            while (dateTime != dateNow)
            {
                StatusDay statusDay;
                if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    statusDay = StatusDay.PreWorking;
                }
                else
                {
                    statusDay = StatusDay.Working;
                }
                days.Add(new WorkingDay()
                {
                    Date = dateTime,
                    Status = statusDay,
                    Hours = 5
                });

                dateTime = dateTime.AddDays(1);
            }

            var tasks = new List<Task>();
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-21),
                EndDateTime = dateNow.AddDays(-2)
            });

            var excepted = new double[] { 64.52, 198000, 198000, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }


        [TestMethod]
        public void TestNoWorkingOfTasks()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var dateNow = DateTime.Now;
            var dateTime = dateNow.AddMonths(-1);
            while (dateTime != dateNow)
            {
                days.Add(new WorkingDay()
                {
                    Date = dateTime,
                    Status = StatusDay.Working,
                    Hours = 8
                });

                dateTime = dateTime.AddDays(1);
            }

            var tasks = new List<Task>();

            var excepted = new double[] { 0, 892800, 892800, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }
        [TestMethod]
        public void TestMoreTasks()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var dateNow = DateTime.Now;
            var dateTime = dateNow.AddMonths(-1);
            while (dateTime != dateNow)
            {
                StatusDay statusDay;
                if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    statusDay = StatusDay.PreWorking;
                }
                else
                {
                    statusDay = StatusDay.Working;
                }
                days.Add(new WorkingDay()
                {
                    Date = dateTime,
                    Status = statusDay,
                    Hours = 5
                });

                dateTime = dateTime.AddDays(1);
            }

            var tasks = new List<Task>();
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-21),
                EndDateTime = dateNow.AddDays(-2)
            });
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-16),
                EndDateTime = dateNow.AddDays(-2)
            });
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-14),
                EndDateTime = dateNow.AddDays(-5)
            });

            var excepted = new double[] { 64.52, 198000, 198000, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }
        [TestMethod]
        public void TestAllLosedTasks()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var dateNow = DateTime.Now;
            var dateTime = dateNow.AddMonths(-1);
            while (dateTime != dateNow)
            {
                StatusDay statusDay;
                if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    statusDay = StatusDay.PreWorking;
                }
                else
                {
                    statusDay = StatusDay.Working;
                }
                days.Add(new WorkingDay()
                {
                    Date = dateTime,
                    Status = statusDay,
                    Hours = 5
                });

                dateTime = dateTime.AddDays(1);
            }

            var tasks = new List<Task>();
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-5),
                EndDateTime = dateNow.AddDays(-2)
            });
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-3),
                EndDateTime = dateNow.AddDays(-2)
            });
            tasks.Add(new Task()
            {
                BeginDateTime = dateNow.AddDays(-9),
                EndDateTime = dateNow.AddDays(-5)
            });

            var excepted = new double[] { 25.81, 414000, 414000, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }
        [TestMethod]
        public void TestCsvFiles()
        {
            List<WorkingDay> days = new List<WorkingDay>();

            var works = "05-01-2023:0:8;05-02-2023:0:8;05-03-2023:0:8;05-04-2023:0:8;05-05-2023:0:8;05-06-2023:4:0;05-07-2023:4:0;05-08-2023:0:8;05-09-2023:0:8;05-10-2023:0:8;05-11-2023:0:8;05-12-2023:0:8;05-13-2023:4:0;05-14-2023:4:0;05-15-2023:0:8;05-16-2023:0:8;05-17-2023:0:8;05-18-2023:0:8;05-19-2023:0:8;05-20-2023:4:0;05-21-2023:4:0;05-22-2023:0:8;05-23-2023:0:8;05-24-2023:0:8;05-25-2023:0:8;05-26-2023:0:8;05-27-2023:4:0;05-28-2023:4:0;05-29-2023:0:8;05-30-2023:0:8;05-31-2023:0:8";
            foreach(var item in works.Split(';'))
            {
                var dateNow = item.Split(':');
                var timeNow = dateNow[0].Split('-');
                
                if (dateNow[1] != "0")
                {
                    days.Add(new WorkingDay
                    {
                        Date = new DateTime(int.Parse(timeNow[2]), int.Parse(timeNow[0]), int.Parse(timeNow[1])),
                        Hours = int.Parse(dateNow[2]),
                        Status = StatusDay.Working
                    });
                }
                else
                {
                    days.Add(new WorkingDay
                    {
                        Date = new DateTime(int.Parse(timeNow[2]), int.Parse(timeNow[0]), int.Parse(timeNow[1])),
                        Hours = int.Parse(dateNow[2]),
                        Status = StatusDay.PreWorking
                    });
                }
            }            

            var tasks = new List<Task>();
            tasks.Add(new Task()
            {
                BeginDateTime = new DateTime(2023, 05, 01),
                EndDateTime = new DateTime(2023, 05, 20),
                FullTitle = "Создание интерфейсов и наборов прав пользователе",
                Guid = "ВОДОКАНАЛ-07"
            }); ;

            var excepted = new double[] { 65.22, 230400, 230400, 0, 0 };
            var actual = WorkingTime.CalculateEfficiency(days, tasks);
            Assert.AreEqual((excepted[0], excepted[1], excepted[2], excepted[3], excepted[4]), (actual[0], actual[1], actual[2], actual[3], actual[4]));
        }
    }
}
