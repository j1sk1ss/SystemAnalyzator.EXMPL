using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SystemAnalyzator.EXMPL.OBJECTS;
using SystemAnalyzator.EXMPL.UI;

namespace SystemAnalyzator.EXMPL {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            Processes = new List<Process>();
            AddEmpty();
        }
        public List<Process> Processes { get; }
        private int _processXCount;
        private int _processYCount;
        
        private new const int Height   = 130;
        private const int LineCapacity = 4;
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
    }
}