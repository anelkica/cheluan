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
        public readonly Turtle TurtleEngine;

        [ObservableProperty]
        private string _codeEditorContent;

        public MainWindowViewModel(ILuaService luaService, Turtle turtle)
        {
            _luaService = luaService;
            TurtleEngine = turtle;

            TurtleEngine.X = 400;
            TurtleEngine.Y = 400;
            TurtleEngine.Angle = 0;
        }

        [RelayCommand]
        public void ExecuteCode()
        {
            Result result = _luaService.ExecuteCode(CodeEditorContent);

            if (result.Failed)
            {
                Console.WriteLine($"ERROR: {result.Error}");
            }
        }
    }
}
