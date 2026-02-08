using cheluan.Models;
using MoonSharp.Interpreter;
using System;
using System.IO;

namespace cheluan.Services;

public class LuaService : ILuaService
{
    private readonly Script _Lua;
    private readonly Turtle _turtle;

    public LuaService(Turtle turtle)
    {
        _Lua = new Script();
        _turtle = turtle;

        UserData.RegisterType<Turtle>(); // register turtle type in Lua

        _Lua.Globals["turtle"] = _turtle; // expose it to Lua
    }

    public Result<string> ReadScript(string filePath)
    {
        try
        {
            string code = File.ReadAllText(filePath);
            return Result<string>.Ok(code);
        }
        catch (Exception e)
        {
            return Result<string>.Fail($"Failed to read script: {e.Message}");
        }
    }

    public Result ExecuteCode(string code)
    {
        try
        {
            _Lua.DoString(code);
            return Result.Ok();
        }
        catch (InterpreterException e)
        {
            // lua specific
            return Result.Fail($"Lua Interpreter Error: {e.DecoratedMessage}");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failed to execute Lua: {e.Message}");
        }
    }
}
