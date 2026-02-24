using Avalonia;
using Avalonia.Platform.Storage;
using cheluan.Models;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace cheluan.Services;

public class LuaService : ILuaService
{
    private readonly Turtle _turtle;

    private Func<Turtle>? _turtleSpawner; // hook for MainWindowViewModel, for spawning and tracking new turtles
    public void RegisterSpawner(Func<Turtle> spawner) => _turtleSpawner = spawner;

    // contains a list of lua function signatures and their description, for rendering in UI
    public static readonly IReadOnlyList<DocumentationEntry> DocumentationEntries =
    [
        new() { Signature = "move(distance)",        Description = "Moves turtle forward." },
        new() { Signature = "teleport(x, y)",        Description = "Teleports turtle to (x,y)." },
        new() { Signature = "center()",              Description = "Teleports turtle back to center (0,0)." },

        new() { Signature = "rect(width, height)",   Description = "Draws rectangle." },
        new() { Signature = "circle(radius)",        Description = "Draws circle." },
        new() { Signature = "polygon(sides, size)",  Description = "Draws polygon with number of sides and side length." },

        new() { Signature = "angle(degrees)",        Description = "Sets turtle's absolute angle in degrees." },
        new() { Signature = "turn(degrees)",         Description = "Rotates turtle relative to its current angle." },

        new() { Signature = "pen_size(thickness)",   Description = "Sets pen thickness." },
        new() { Signature = "pen_up()",              Description = "Lifts pen so movement doesn't draw." },
        new() { Signature = "pen_down()",            Description = "Puts pen down so movement draws." },
        new() { Signature = "color(hex)",            Description = "Sets pen color. #RRGGBB format." },
        new() { Signature = "color(r, g, b)",        Description = "Sets pen color. RGB format."}
    ];

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
            if (_turtleSpawner != null)
            {
                Table turtleClass = new(script);
                turtleClass["spawn"] = (Func<Turtle>)(() => _turtleSpawner());

                script.Globals["turtle_spawner"] = turtleClass;
            }

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