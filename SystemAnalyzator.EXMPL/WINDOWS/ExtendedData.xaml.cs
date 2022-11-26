using System.Windows;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL.WINDOWS {
    public partial class ExtendedData : Window {
        public ExtendedData(Process process) {
            InitializeComponent();
            DayStat.Series = new SeriesCollection() {
                new LineSeries() {
                    Title = process.Name,
                    Values = process.Statistic.DayUsage.AsChartValues()
                }
            };
            MonthStat.Series = new SeriesCollection() {
                new LineSeries() {
                    Title = process.Name,
                    Values = process.Statistic.MonthUsage.AsChartValues()
                }
            };
        }
    }
}