using cheluan.Models;
using cheluan.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace cheluan.ViewModels    
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ILuaService _luaService;
        private readonly Turtle _turtle;

        [ObservableProperty]
        private string? _inputText; // generates public string InputText { get; set; }

        [ObservableProperty]
        private string? _displayText;

        [RelayCommand]
        private void MoveLua()
        {
            Result result = _luaService.ExecuteCode("turtle.Move(50)");

            if (result.success)
            {
                Console.WriteLine("Executed: turtle.Move(50)");
            }
            else
            {
                Console.WriteLine(result.error);
            }
        }

        public MainWindowViewModel(ILuaService luaService, Turtle turtle)
        {
            _luaService = luaService;
            _turtle = turtle;
        }
    }
}
