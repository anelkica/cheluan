using Avalonia.Controls;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;
using cheluan.Models;
using cheluan.ViewModels;
using System;
using TextMateSharp.Grammars;

namespace cheluan.Views
{
    public partial class MainWindow : Window
    {
        private TextMate.Installation _textMateInstallation;
        private readonly TurtleRenderer _renderer;

        public Action? RequestClearCanvas { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            RegistryOptions registryOptions = new(ThemeName.Dracula);
            _textMateInstallation = CodeEditor.InstallTextMate(registryOptions);
            _textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId("lua"));

            _renderer = new TurtleRenderer(TurtleCanvas);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (DataContext is not MainWindowViewModel vm) return;

            Turtle turtle = vm.TurtleEngine;

            vm.RequestClearCanvas = _renderer.Clear;
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