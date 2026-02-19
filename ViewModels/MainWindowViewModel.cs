using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaEdit;
using cheluan.Models;
using cheluan.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace cheluan.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public INotificationService Notification { get; }
        private readonly ILuaService _luaService;

        public event Action<Turtle>? TurtleCreated;
        public ObservableCollection<Turtle> Turtles { get; } = new();
        public readonly Turtle MainTurtle;

        // these properties are set by MainView
        public TextEditor? CodeEditor { get; set; }
        public Visual? VisualRoot { get; set; } // needed for TopLevel StorageProvider access

        // if the file is opened/existing, don't create unneeded save file dialogs
        private IStorageFile? _currentFile;

        // for the MainWindow, clearing canvas
        public event Action? ClearCanvasRequested;

        [ObservableProperty] private string _titlebarText = "cheluan"; // ─
        [ObservableProperty] private bool _saved = true; //  false when user types in editor (MainWindow)

        partial void OnSavedChanged(bool value) => UpdateTitlebar();

        public MainWindowViewModel(INotificationService notificationService, ILuaService luaService, Turtle turtle)
        {
            Notification = notificationService;
            _luaService = luaService;
            MainTurtle = turtle;

            Turtles.Add(MainTurtle);
            luaService.RegisterSpawner(CreateNewTurtle);
        }

        // -- HELPERS -- //
        public Turtle CreateNewTurtle()
        {
            Turtle turtle = new() { Bounds = MainTurtle.Bounds };
            turtle.Reset();

            Turtles.Add(turtle);
            TurtleCreated?.Invoke(turtle);

            return turtle;
        }

        private void UpdateTitlebar()
        {
            string filename = _currentFile?.Name ?? "untitled.lua";
            string unsavedMarker = !Saved ? "*" : "";

            TitlebarText = $"{filename}{unsavedMarker} — cheluan";
        }

        // -- UI HANDLERS -- //
        [RelayCommand]
        public async Task RequestCanvasClear()
        {
            Turtles.Clear();

            MainTurtle.Reset();
            Turtles.Add(MainTurtle);

            ClearCanvasRequested?.Invoke(); // connected in MainView
        }

        [RelayCommand]
        public async Task ExecuteCodeFromEditor()
        {
            if (CodeEditor is null)
            {
                await Notification.NotifyAsync(Result.Fail("Code editor isn't loaded"));
                return;
            }

            await RequestCanvasClear();

            Result result = _luaService.ExecuteCode(CodeEditor.Text);
            if (result.Failed)
            {
                await Notification.NotifyAsync(result);
                return;
            }

            //await Notification.NotifyAsync(result, "Successfully ran.");
        }

        [RelayCommand]
        public async Task NewFileAsync()
        {
            await RequestCanvasClear();

            if (CodeEditor is not null)
                CodeEditor?.Text = "";

            _currentFile = null;
            Saved = false;

            UpdateTitlebar();
        }

        [RelayCommand]
        public async Task OpenFileAsync()
        {
            TopLevel? topLevel = TopLevel.GetTopLevel(VisualRoot);
            if (topLevel is null) return;

            IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Lua Script",
                FileTypeFilter = [new FilePickerFileType("Lua Files") { Patterns = ["*.lua"] }],
                AllowMultiple = false
            });

            if (files is not { Count: > 0 }) return;

            _currentFile = files[0];

            Result<string> result = await _luaService.ReadScriptFileAsync(_currentFile);
            if (result.Success)
                CodeEditor?.Text = result.Value ?? ""; // null content? default to ""

            Saved = true; // refactor later im tired
            await RequestCanvasClear();
        }

        [RelayCommand]
        public async Task SaveFileAsync() // it fires on CTRL+S too
        {
            if (CodeEditor is null) return;

            if (_currentFile != null)
                await _luaService.SaveScriptFileAsync(_currentFile, CodeEditor?.Text ?? "");
            else
                await SaveAsFileAsync();  // file doesn't exist? do Save As

            Saved = true;
            await ExecuteCodeFromEditor();
        }

        [RelayCommand]
        public async Task SaveAsFileAsync()
        {
            TopLevel? topLevel = TopLevel.GetTopLevel(VisualRoot);
            if (topLevel == null) return;


            FilePickerFileType luaFileType = new FilePickerFileType("Lua Scripts")
            {
                Patterns = ["*.lua"],
                MimeTypes = ["text/x-lua", "text/plain"]
            };

            IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Lua Script",
                DefaultExtension = ".lua",
                SuggestedFileName = "script.lua",
                FileTypeChoices = [luaFileType]
            });

            if (file is null) return;

            _currentFile = file;

            await _luaService.SaveScriptFileAsync(_currentFile, CodeEditor?.Text ?? "");
        }
    }
}