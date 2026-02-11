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
        private readonly TurtleRenderer _renderer;

        public MainWindow()
        {
            InitializeComponent();

            _renderer = new TurtleRenderer(TurtleCanvas);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (DataContext is not MainWindowViewModel vm) return;

            Turtle turtle = vm.TurtleEngine;

            turtle.OnMove = _renderer.DrawStep;
        }

        // diy titlebar
        private void Titlebar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }
    }
}