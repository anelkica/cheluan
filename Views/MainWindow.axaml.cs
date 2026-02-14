using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using AvaloniaEdit.TextMate;
using cheluan.Models;
using cheluan.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using TextMateSharp.Grammars;

namespace cheluan.Views
{
    public partial class MainWindow : Window
    {
        private TextMate.Installation _textMateInstallation;
        private readonly TurtleRenderer _turtleRenderer;

        public MainWindow()
        {
            InitializeComponent();

            RegistryOptions registryOptions = new(ThemeName.Dracula);
            _textMateInstallation = CodeEditor.InstallTextMate(registryOptions);
            _textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId("lua"));

            _turtleRenderer = new TurtleRenderer(TurtleCanvas);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (DataContext is not MainWindowViewModel vm) return;

            vm.VisualRoot = this;
            vm.CodeEditor = CodeEditor;

            Turtle turtle = vm.TurtleEngine;

            turtle.Bounds = new(TurtleCanvas.Width, TurtleCanvas.Height);
            turtle.Reset();

            vm.ClearCanvasRequested += OnClearCanvasRequested;
            turtle.OnMove += _turtleRenderer.DrawStep;

            CodeEditor.TextChanged += (s, e) => vm.Saved = false;
        }

        // -- UI HANDLERS -- //

        // diy titlebar drag mechanics
        private void Titlebar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        // -- HELPERS -- //
        private void OnClearCanvasRequested()
        {
            if (DataContext is not MainWindowViewModel vm) return;

            vm.TurtleEngine.Reset();
            _turtleRenderer.Clear();
        }
    }
}