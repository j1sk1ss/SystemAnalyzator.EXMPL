using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json;
using SystemAnalyzator.EXMPL.DATA;
using SystemAnalyzator.EXMPL.OBJECTS;
using SystemAnalyzator.EXMPL.UI;

namespace SystemAnalyzator.EXMPL {
    public partial class MainWindow {
        private const string DataLocation = "Processes.json";
        public MainWindow() {
            InitializeComponent();
            Processes = new List<Process>();
            Data      = new Data();

            _timer.Tick += HourStatistic;
            
            AddEmpty();
            
            if (!File.Exists(DataLocation)) return;
            try {
                Data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(DataLocation));
                Processes = Data!.Processes;

                foreach (var process in Processes) {
                    process.InterfaceBody = ProcessTemplate.GetEmptyProcess(new Process(this));
                    process.MainWindow = this;
                    process.SetProcess(null, null);
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

        private void HourStatistic(object sender, EventArgs eventArgs) {
            foreach (var process in Processes) {
                process.Statistic.AddHour(process.WorkTime);
            }
        }
        
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
            
            SetProcess(ProcessTemplate.GetEmptyProcess(new Process(this)), _processXCount++, _processYCount);
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