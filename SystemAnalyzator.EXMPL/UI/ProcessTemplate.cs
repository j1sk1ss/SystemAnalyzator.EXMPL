using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL.UI {
    public class ProcessTemplate {
        public static Grid GetEmptyProcess(Process process) {
            var tempGrid = new Grid() {
                Width               = 100, 
                Height              = 120,
                VerticalAlignment   = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Children = {
                    new Line() {
                       X1 = 0,
                       Y1 = 0,
                       X2 = 100,
                       Y2 = 0,
                       Stroke = Brushes.Black
                    },
                    new Line() {
                        X1 = 0,
                        Y1 = 120,
                        X2 = 100,
                        Y2 = 120,
                        Stroke = Brushes.Black
                    },
                    new Line() {
                        X1 = 0,
                        Y1 = 120,
                        X2 = 0,
                        Y2 = 0,
                        Stroke = Brushes.Black
                    },
                    new Line() {
                        X1 = 100,
                        Y1 = 120,
                        X2 = 100,
                        Y2 = 0,
                        Stroke = Brushes.Black
                    },
                    new Image() {
                        Height              = 60,
                        Width               = 50,
                        Stretch             = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment   = VerticalAlignment.Center
                    }
                }
            };
            tempGrid.Children.Add(new Button() {
                Height  = 20,
                Width   = 20,
                Content = '+'
            });
            (tempGrid.Children[^1] as Button)!.Click += process.SetProcess;
            process.InterfaceBody = tempGrid;
            return process.InterfaceBody;
        }
    }
}