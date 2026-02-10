using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using cheluan.Models;
using cheluan.ViewModels;
using System;

namespace cheluan.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (DataContext is not MainWindowViewModel vm) return;

            Turtle turtle = vm.TurtleEngine;

            turtle.OnMove = OnTurtleMove;
        }

        // diy titlebar
        private void Titlebar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private void OnTurtleMove(TurtleStep step)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Line line = new Line
                {
                    StartPoint = new Avalonia.Point(step.StartX, step.StartY),
                    EndPoint = new Avalonia.Point(step.EndX, step.EndY),
                    Stroke = Brushes.LawnGreen,
                    StrokeThickness = 2,
                    StrokeJoin = PenLineJoin.Round,
                    StrokeLineCap = PenLineCap.Round
                };

                // Adding it to your named Canvas
                TurtleCanvas.Children.Add(line);
            });
        }
    }
}