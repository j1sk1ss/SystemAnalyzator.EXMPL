using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemAnalyzator.EXMPL.DATA {
    public class Statistic {
        public Statistic() {
            DayUsage = new List<int>();
            MonthUsage = new List<int>();
        }
        public List<int> DayUsage { get; set; }
        public List<int> MonthUsage { get; set; }
        public void AddHour(DateTime hourTime) {
            if (DayUsage.Count < 24) DayUsage.Add(hourTime.Hour);
            else {
                AddMonth();
                DayUsage.Clear();
                DayUsage.Add(hourTime.Hour);
            }
        }
        private void AddMonth() {
            if (MonthUsage.Count < 30) MonthUsage.Add(DayUsage.Sum(x => x));
            else {
                MonthUsage.Clear();
                MonthUsage.Add(DayUsage.Sum(x => x));
            }
        }
    }
}