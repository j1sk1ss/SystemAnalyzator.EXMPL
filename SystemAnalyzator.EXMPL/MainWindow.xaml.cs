using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using SystemAnalyzator.EXMPL.DATA;
using SystemAnalyzator.EXMPL.FRONTEND;
using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL {
    public partial class MainWindow {
        private const string DataLocation = "Processes.json";
        public MainWindow() {
            InitializeComponent();
            Processes = new List<Process>();
            Data      = new Data();

            _timer.Tick         += HourStatistic;
            _statistic.Tick     += UpdatePieChart;
            _processesList.Tick += SetProcessesList;
            
            _timer.IsEnabled         = true;
            _statistic.IsEnabled     = true;
            _processesList.IsEnabled = true;
            
            AddEmpty();
            
            if (!File.Exists(DataLocation)) return;
            try {
                Data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(DataLocation));
                Processes = Data!.Processes;

                foreach (var process in Processes) {
                    process.MainWindow = this;
                    process.SetProcess(null, null);
                    process.InterfaceBody = ProcessTemplate.GetEmptyProcess(process, false);
                }

                UpdateProcesses();
                AddEmpty();
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}");
                throw;
            }
        }
        public List<Process> Processes { get; set; }
        private Data Data { get; }

        private int _processXCount;
        private int _processYCount;
        
        private new const int Height   = 130;
        private const int LineCapacity = 4;
        
        private readonly DispatcherTimer _timer = new () {
            Interval = new TimeSpan(0,1,0,0)
        };
        private readonly DispatcherTimer _statistic = new () {
            Interval = new TimeSpan(0,0,0,1)
        };
        private readonly DispatcherTimer _processesList = new () {
            Interval = new TimeSpan(0,0,0,1)
        };
        public void UpdateProcesses() {
            ProcessesSpace.Children.Clear();
            ProcessesSpace.Height = Height + Height * Processes.Count / LineCapacity;
            
            var x = 0;
            var y = 0;
            
            foreach (var process in Processes) {
                SetProcess(process.InterfaceBody, x, y);
                if (++x <= LineCapacity) continue;
                x = 0;
                y++;
            }

            _processXCount = x;
            _processYCount = y;
        }
        public void DeleteProcess() {
            _processXCount -= 2;
            if (_processXCount >= 0) return;
            _processXCount = LineCapacity;
            _processYCount--;
        }
        public void AddEmpty() {
            if (_processXCount > LineCapacity) {
                _processXCount = 0;
                ProcessesSpace.Height = Height + Height * ++_processYCount;
            }
            
            SetProcess(ProcessTemplate.GetEmptyProcess(new Process(this), true), _processXCount++, _processYCount);
        }
        private void SetProcessesList(object sender, EventArgs eventArgs) {
            var processes = System.Diagnostics.Process.GetProcesses();
            
            ProcessesList.Content = "";
            for (var i = 0; i < processes.Length; i++) {
                ProcessesList.Content += $"{i + 1}) {processes[i].ProcessName}\n";
            }
        }
        private void UpdatePieChart(object sender, EventArgs eventArgs) {
            var timeLst = Processes.Select(process => process.WorkTime.Second).ToList();
            var nameList = Processes.Select(process => process.Name).ToList();

            var pieSeries = RealTimeStatistic.Series;
            
            if (pieSeries.Count == 0 || pieSeries.Count != timeLst.Count) {
                RealTimeStatistic.Series.Clear();
                for (var i = 0; i < timeLst.Count; i++) {
                    RealTimeStatistic.Series.Add(
                        new PieSeries {
                            Title = nameList[i],
                            Values = new ChartValues<int> {timeLst[i]},
                            DataContext = this
                        }
                    );
                }
            }
            else {
                for (var i = 0; i < timeLst.Count; i++) {
                    pieSeries[i].ActualValues[0] = timeLst[i];
                }
            }
        }
        private void HourStatistic(object sender, EventArgs eventArgs) {
            foreach (var process in Processes) {
                process.Statistic.AddHour(process.WorkTime);
            }
        }
        private void SetProcess(UIElement processGrid, int x, int y) {
            ProcessesSpace.Children.Add(processGrid);
            (ProcessesSpace.Children[^1] as Grid)!.Margin = new Thickness(100 * x, 120 * y, 0, 0);
        }
        private void ProgramClosed(object sender, EventArgs e) {
            try {
                Data.Processes = Processes;
                File.WriteAllText(DataLocation, JsonConvert.SerializeObject(Data));
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}");
                throw;
            }
        }
    }
}