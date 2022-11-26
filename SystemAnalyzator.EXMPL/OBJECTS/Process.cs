using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using SystemAnalyzator.EXMPL.DATA;
using SystemAnalyzator.EXMPL.FRONTEND;
using SystemAnalyzator.EXMPL.WINDOWS;

namespace SystemAnalyzator.EXMPL.OBJECTS {
    public class Process {
        public Process(MainWindow mainWindow) {
            MainWindow    = mainWindow;
            InterfaceBody = new Grid();
            WorkTime      = new DateTime();
            Statistic     = new Statistic();
        }
        public Statistic Statistic { get; }
        public DateTime WorkTime { get; set; }
        [JsonIgnore]
        public Grid InterfaceBody { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public System.Diagnostics.Process[] ProcessBody { get; set; }
        [JsonIgnore]
        public MainWindow MainWindow { get; set; }
        [JsonIgnore]
        public Label Time { get; set; }
        
        private readonly DispatcherTimer _timer = new () {
            Interval = new TimeSpan(0,0,0,1)
        };
        public void Delete(object sender, RoutedEventArgs routedEventArgs) {
            MainWindow.Processes.Remove(this);
            MainWindow.DeleteProcess();
            MainWindow.UpdateProcesses();
            MainWindow.AddEmpty();
            
            _timer.IsEnabled = false;
        }
        public void ShowData(object sender, RoutedEventArgs routedEventArgs) {
            new ExtendedData(this).Show();
        }
        public void SetProcess(object sender, RoutedEventArgs e) {
            if (sender != null) {
                var chosenProcess = new OpenFileDialog();
                if (chosenProcess.ShowDialog() != true) return;
                Name = chosenProcess.SafeFileName;
                
                MainWindow.Processes.Add(this);
                MainWindow.AddEmpty();
                (sender as Button)!.Visibility = Visibility.Hidden;
            }
            
            ProcessBody = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Name ?? " "));
            
            _timer.Tick += CheckProcess;
            _timer.IsEnabled = true;
            
            ProcessTemplate.AddInterface(this);
        }
        private void CheckProcess(object sender, EventArgs eventArgs) {
            ProcessBody = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Name));
            if (ProcessBody.Length == 0) return;
            var addSeconds = WorkTime.AddSeconds(1);
            WorkTime     = addSeconds;
            Time.Content = $"{WorkTime:mm:ss}";
        }
    }
}