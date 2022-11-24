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

namespace SystemAnalyzator.EXMPL {
    public partial class MainWindow : Window {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
// Программа отслеживает все процессы на компьютере и выводит графики 
// Возможность задавать свои отслеживаемые процессы 
// Круговой график общего времени со всеми процессами
// Дневной график использования процессора + памяти и тд (делаем вывод что пк активен)
// Месячный график средней активности
 