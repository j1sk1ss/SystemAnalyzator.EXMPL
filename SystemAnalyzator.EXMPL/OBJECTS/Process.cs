using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using SystemAnalyzator.EXMPL.DATA;

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
        private System.Diagnostics.Process[] ProcessBody { get; set; }
        [JsonIgnore]
        public MainWindow MainWindow { get; set; }
        private Label Time { get; set; }
        
        private readonly DispatcherTimer _timer = new () {
            Interval = new TimeSpan(0,0,0,1)
        };
        private void AddInterface() {
            InterfaceBody.Children.Add(new Label {
                Content = (ProcessBody ?? Array.Empty<System.Diagnostics.Process>()).Length > 0 
                    ? ProcessBody[^1].ProcessName : "NOT FOUND!",
                HorizontalAlignment = HorizontalAlignment.Center
            });
            
            Time = new Label {
                Content = "сек",
                Margin = new Thickness(0,85,0,0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            InterfaceBody.Children.Add(Time);
            
            var deleteButton = new Button {
                Height  = 20,
                Width   = 75,
                Content = "Удалить"
            };
            deleteButton.Click += Delete;
            InterfaceBody.Children.Add(deleteButton);
        }
        private void Delete(object sender, RoutedEventArgs routedEventArgs) {
            MainWindow.Processes.Remove(this);
            _timer.IsEnabled = false;
            MainWindow.DeleteProcess();
            MainWindow.UpdateProcesses();
            MainWindow.AddEmpty();
        }
        public void SetProcess(object sender, RoutedEventArgs e) {
            if (sender != null) {
                var chosenProcess = new OpenFileDialog();
                if (chosenProcess.ShowDialog() != true) return;
                Name = chosenProcess.SafeFileName;
            }
            ProcessBody = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Name ?? " "));
            
            AddInterface();

            if (sender != null) {
                MainWindow.Processes.Add(this);
                MainWindow.AddEmpty();
                (sender as Button)!.Visibility = Visibility.Hidden;
            }
            
            _timer.Tick += CheckProcess;
            _timer.IsEnabled = true;
        }
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", 
            MessageId = "type: System.Diagnostics.ThreadInfo")]
        private void CheckProcess(object sender, EventArgs eventArgs) {
            ProcessBody = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Name));
            if (ProcessBody.Length == 0) return;
            var addSeconds = WorkTime.AddSeconds(1);
            WorkTime = addSeconds;
            Time.Content = $"{WorkTime:mm:ss}";
        }
    }
}