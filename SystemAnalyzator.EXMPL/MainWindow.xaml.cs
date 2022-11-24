using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemAnalyzator.EXMPL.OBJECTS;
using SystemAnalyzator.EXMPL.UI;

namespace SystemAnalyzator.EXMPL {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Processes = new List<Process>();
            AddEmpty();
        }
        public List<Process> Processes { get; set; }
        private int _processXCount;
        private int _processYCount;
        public void UpdateProcesses() {
            ProcessesSpace.Children.Clear();
            ProcessesSpace.Height = 130 + 130 * Processes.Count / 5;
            var x = 0;
            var y = 0;
            foreach (var t in Processes) {
                ProcessesSpace.Children.Add(t.InterfaceBody);
                (ProcessesSpace.Children[^1] as Grid)!.Margin = new Thickness(100 * x, 120 * y, 0, 0);
                if (++x <= 4) continue;
                x = 0;
                y++;
            }

            _processXCount -= 2;
            if (_processXCount >= 0) return;
            _processXCount = 4;
            _processYCount -= 1;
        } 
        public void AddEmpty() {
            if (_processXCount > 4) {
                _processXCount = 0;
                _processYCount += 1;
            }
            ProcessesSpace.Height = 130 + 130 * _processYCount;
            ProcessesSpace.Children.Add(ProcessTemplate.GetEmptyProcess(new Process(this)));
            (ProcessesSpace.Children[^1] as Grid)!.Margin = new Thickness(100*_processXCount, 120*_processYCount, 0, 0);
            _processXCount++;
        }
    }
}
// Программа отслеживает все процессы на компьютере и выводит графики 
// Возможность задавать свои отслеживаемые процессы 
// Круговой график общего времени со всеми процессами
// Дневной график использования процессора + памяти и тд (делаем вывод что пк активен)
// Месячный график средней активности
 