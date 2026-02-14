using cheluan.Models;
using MoonSharp.Interpreter;
using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace cheluan.Services;

public class LuaService : ILuaService
{
    private readonly Turtle _turtle;

    public LuaService(Turtle turtle)
    {
        _turtle = turtle;
        UserData.RegisterType<Turtle>();
    }

    public Result ExecuteCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Fail("No code to execute");

        try
        {
            Script script = new();
            script.Globals["turtle"] = _turtle;
            script.DoString(code);
            return Result.Ok();
        }
        catch (InterpreterException e)
        {
            return Result.Fail($"Lua Interpreter Error: {e.DecoratedMessage}");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failed to execute Lua: {e.Message}");
        }
    }

    public async Task<Result<string>> ReadScriptFileAsync(IStorageFile file)
    {
        if (file == null)
            return Result<string>.Fail("No file selected");

        try
        {
            await using Stream stream = await file.OpenReadAsync();
            using StreamReader reader = new(stream);
            string content = await reader.ReadToEndAsync();

            return Result<string>.Ok(content);
        }
        catch (Exception e)
        {
            return Result<string>.Fail($"Failed to read script: {e.Message}");
        }
    }

    public async Task<Result> SaveScriptFileAsync(IStorageFile file, string content)
    {
        if (file == null)
            return Result.Fail("No destination file selected");

        content ??= "";

        try
        {
            await using Stream stream = await file.OpenWriteAsync();
            await using StreamWriter writer = new(stream);
            await writer.WriteAsync(content);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail($"Failed to save script: {e.Message}");
        }
    }
}