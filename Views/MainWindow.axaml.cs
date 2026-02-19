using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
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

            vm.ClearCanvasRequested += OnClearCanvasRequested;
            vm.ExportCanvasRequested += OnExportCanvasRequested;

            SetupTurtle(vm.MainTurtle);
            vm.TurtleCreated += SetupTurtle;

            CodeEditor.TextChanged += (s, e) => vm.Saved = false;
            PropertyChanged += (s, e) =>
            {
                if (e.Property.Name == nameof(WindowState))
                    AdjustTitlebarHeight();
            };

            AdjustTitlebarHeight();
        }

        // -- HELPERS -- //
        private void SetupTurtle(Turtle turtle)
        {
            turtle.Bounds = new(TurtleCanvas.Width, TurtleCanvas.Height);
            turtle.Reset();

            turtle.OnMove += _turtleRenderer.DrawStep;
        }

        private void OnClearCanvasRequested()
        {
            if (DataContext is not MainWindowViewModel vm) return;

            vm.MainTurtle.Reset();
            _turtleRenderer.Clear();
        }

        private async void OnExportCanvasRequested()
        {
            IStorageFile? file = await this.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export Canvas",
                DefaultExtension = ".png",
                SuggestedFileName = "export.png",
                FileTypeChoices = [new FilePickerFileType("PNG Image") { Patterns = ["*.png"] }]
            });

            if (file is null) return;

            PixelSize canvasPixelSize = new((int)TurtleCanvas.Width, (int)TurtleCanvas.Height);
            using RenderTargetBitmap bitmap = new(canvasPixelSize, new Vector(96, 96));

            bitmap.Render(TurtleCanvas);

            using Stream stream = await file.OpenWriteAsync();
            bitmap.Save(stream);

            // add a notification here??
        }

        // for adjusting maximized titlebar height (especially for people with top-screen taskbars)
        private void AdjustTitlebarHeight()
        {
            var titlebarBorder = this.FindControl<Border>("Titlebar");
            if (titlebarBorder == null) return;

            if (WindowState == WindowState.Maximized)
            {
                titlebarBorder.Margin = new(0, 7, 0, 0);
            }
            else
            {
                titlebarBorder.Margin = new(0);
            }
        }

        // -- WINDOW CONTROLS -- //
        private void Titlebar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}