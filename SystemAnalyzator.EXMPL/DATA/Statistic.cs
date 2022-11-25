using System;
using System.Collections.Generic;

namespace SystemAnalyzator.EXMPL.DATA {
    public class Statistic {
        public Statistic() {
            DayUsage = new List<DateTime>();
            MonthUsage = new List<List<DateTime>>();
        }
        public List<DateTime> DayUsage { get; set; }
        public List<List<DateTime>> MonthUsage { get; set; }

        public void AddHour(DateTime hourTime) {
            if (DayUsage.Count < 24) DayUsage.Add(hourTime);
            else {
                AddMonth();
                DayUsage.Clear();
                DayUsage.Add(hourTime);
            }
        }

        public void AddMonth() {
            if (MonthUsage.Count < 30) MonthUsage.Add(DayUsage);
            else {
                MonthUsage.Clear();
                MonthUsage.Add(DayUsage);
            }
        }
    }
}