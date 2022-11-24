using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SystemAnalyzator.EXMPL.OBJECTS {
    public class Process {
        public Process(MainWindow mainWindow) {
            MainWindow = mainWindow;
        }
        private MainWindow MainWindow { get; set; }
        public Grid InterfaceBody { get; set; }
        public System.Diagnostics.Process[] ProcessBody { get; set; }
        public Image ProcessIcon { get; set; }
        public string ProcessName { get; set; }
        private void AddInterface() {
            var deleteButton = new Button() {
                Height = 20,
                Width = 75,
                Content = "Удалить"
            };
            deleteButton.Click += Delete;
            
            InterfaceBody.Children.Add(new Label() {
                Content = ProcessBody.Length > 0 ? ProcessBody[^1].ProcessName : "NULL"
            });
            InterfaceBody.Children.Add(deleteButton);
        }
        private void Delete(object sender, RoutedEventArgs routedEventArgs) {
            MainWindow.Processes.Remove(this);
            MainWindow.UpdateProcesses();
            MainWindow.AddEmpty();
        }
        public void SetProcess(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            ProcessBody = System.Diagnostics.Process.GetProcessesByName(openFileDialog.SafeFileName);
            AddInterface();
            (sender as Button)!.Visibility = Visibility.Hidden;
            MainWindow.Processes.Add(this);
            MainWindow.AddEmpty();
        }
    }
}