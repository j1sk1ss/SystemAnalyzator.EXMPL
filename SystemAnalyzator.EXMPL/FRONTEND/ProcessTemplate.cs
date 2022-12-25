using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL.FRONTEND {
    public static class ProcessTemplate {
        public static Grid GetEmptyProcess(Process process, bool isNew) {
            var tempGrid = new Grid {
                Width               = 100, 
                Height              = 120,
                VerticalAlignment   = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Children = {
                    new Line {
                       X1 = 0,
                       Y1 = 0,
                       X2 = 100,
                       Y2 = 0,
                       Stroke = Brushes.Black
                    },
                    new Line {
                        X1 = 0,
                        Y1 = 120,
                        X2 = 100,
                        Y2 = 120,
                        Stroke = Brushes.Black
                    },
                    new Line {
                        X1 = 0,
                        Y1 = 120,
                        X2 = 0,
                        Y2 = 0,
                        Stroke = Brushes.Black
                    },
                    new Line {
                        X1 = 100,
                        Y1 = 120,
                        X2 = 100,
                        Y2 = 0,
                        Stroke = Brushes.Black
                    },
                    new Image {
                        Height              = 60,
                        Width               = 50,
                        Stretch             = Stretch.Fill,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment   = VerticalAlignment.Center
                    }
                }
            };
            if (isNew) {
                tempGrid.Children.Add(new Button() {
                    Height  = 20,
                    Width   = 20,
                    Content = '+'
                });
                (tempGrid.Children[^1] as Button)!.Click += process.SetProcess;
                process.InterfaceBody = tempGrid;
            }
            else {
                process.InterfaceBody = tempGrid;
                AddInterface(process);
            }
            
            return process.InterfaceBody;
        }
        
        public static void AddInterface(Process process) {
            process.InterfaceBody.Children.Add(new Label {
                Content = (process.ProcessBody ?? Array.Empty<System.Diagnostics.Process>()).Length > 0 
                    ? process.ProcessBody[^1].ProcessName : "NOT FOUND!",
                HorizontalAlignment = HorizontalAlignment.Center
            });
            
            process.Time = new Label {
                Content = "сек",
                Margin  = new Thickness(0,85,0,0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            process.InterfaceBody.Children.Add(process.Time);
            
            var deleteButton = new Button {
                Height  = 20,
                Width   = 75,
                Content = "Удалить"
            };
            deleteButton.Click += process.Delete;
            process.InterfaceBody.Children.Add(deleteButton);
            
            var infoButton = new Button {
                Height  = 20,
                Width   = 75,
                Content = "Статистика"
            };
            infoButton.Click += process.ShowData;
            infoButton.Margin = new Thickness(0, 40, 0, 0);
            process.InterfaceBody.Children.Add(infoButton);
        }
    }
}