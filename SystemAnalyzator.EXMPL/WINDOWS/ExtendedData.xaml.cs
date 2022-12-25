using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Helpers;

using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL.WINDOWS {
    public partial class ExtendedData {
        public ExtendedData(Process process) {
            InitializeComponent();
            
            DayStat.Series = new SeriesCollection {
                new LineSeries {
                    Title = process.Name,
                    Values = process.Statistic.DayUsage.AsChartValues()
                }
            };
            
            MonthStat.Series = new SeriesCollection {
                new LineSeries {
                    Title = process.Name,
                    Values = process.Statistic.MonthUsage.AsChartValues()
                }
            };
        }
    }
}