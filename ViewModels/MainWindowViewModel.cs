using AvaloniaEdit;
using cheluan.Models;
using cheluan.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace cheluan.ViewModels    
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public INotificationService Notification { get; }

        private readonly ILuaService _luaService;
        public readonly Turtle TurtleEngine;

        public Action? RequestClearCanvas { get; set; }

        [ObservableProperty]
        private bool _autoClear = false;

        public MainWindowViewModel(INotificationService notificationService, ILuaService luaService, Turtle turtle)
        {
            Notification = notificationService;
            _luaService = luaService;
            TurtleEngine = turtle;

            TurtleEngine.X = 400;
            TurtleEngine.Y = 400;
            TurtleEngine.Angle = 0;
        }

        [RelayCommand]
        public async void ExecuteCode(TextEditor CodeEditor)
        {
            if (AutoClear) // automatically clear canvas
            {
                RequestClearCanvas?.Invoke();
            }

            Result result = _luaService.ExecuteCode(CodeEditor.Text);

            if (result.Failed)
            {
                Console.WriteLine($"ERROR: {result.Error}");
            }

            
            await Notification.NotifyAsync(result, "Turtle Moved!");
        }
    }
}
