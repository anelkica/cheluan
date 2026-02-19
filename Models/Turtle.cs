using Avalonia;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;

namespace cheluan.Models;

// a record tracking the movement of the turtle from start to end of a line
public record TurtleStep(Point StartPoint, Point EndPoint, string PenColorHex, double PenSize);

[MoonSharpUserData]
public class Turtle
{
    public Point Position { get; set; }
    public Size Bounds { get; set; } // safety against crossing the available canvas
    public double Angle { get; set; }
    public double AngleRadians => Angle * Math.PI / 180;

    public string PenColor { get; private set; } = "#32CD32";
    public double PenSize { get; private set; } = 2;
    public bool IsPenDown { get; private set; } = true;

    public event Action<TurtleStep>? OnMove;

    public void Reset()
    {
        Position = new(Bounds.Width / 2, Bounds.Height / 2); // centers the turtle
        Angle = 0;

        PenSize = 2;
        IsPenDown = true;
    }

    // move forwards
    public Result Move(double distance)
    {
        Point oldPos = Position;

        double dx = distance * Math.Cos(AngleRadians);
        double dy = -distance * Math.Sin(AngleRadians); // flip the Y because +Y means up but screen space is inverted

        Point newPos = new(Position.X + dx, Position.Y + dy);

        if (newPos.X < 0 || newPos.Y < 0 || newPos.X > Bounds.Width || newPos.Y > Bounds.Height)
            return Result.Fail($"Turtle out of bounds: ({newPos.X:F1},{newPos.Y:F1})! Bounds: ({Bounds.Width},{Bounds.Height})");

        Position = newPos;

        if (IsPenDown)
            OnMove?.Invoke(new TurtleStep(oldPos, newPos, PenColor, PenSize));

        return Result.Ok();
    }

    public Result Teleport(double localCoordinateX, double localCoordinateY)
    {
        // convert to absolute/canvas coordinates, bcuz local (0,0) means (200,200) in absolute cords (default canvas is 400x400)
        double canvasX = (Bounds.Width / 2) + localCoordinateX;
        double canvasY = (Bounds.Height / 2) - localCoordinateY; // invert Y so it goes up on the screen

        if (canvasX < 0 || canvasX > Bounds.Width || canvasY < 0 || canvasY > Bounds.Height)
            return Result.Fail($"Position ({canvasX}, {canvasY}) is outside the visible area");

        Position = new Point(canvasX, canvasY);
        return Result.Ok();
    }

    public Result Color(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            return Result.Fail("Hex color is undefined");

        if (!hex.StartsWith("#")) // RRGGBB format maybe?
            hex = "#" + hex;

        if (hex.Length == 7 || hex.Length == 9) // #RRGGBB || #AARRGGBB
            PenColor = hex;

        return Result.Ok();
    }

    public void Turn(double degrees) => SetAngle(Angle + degrees); // relative turning
    public void SetAngle(double degrees) => Angle = (degrees % 360 + 360) % 360; // sets the absolute angle (always positive btw)

    public void SetPenSize(double thickness) => PenSize = thickness < 1 ? 1 : thickness; // thickness cant be less than 1
    public void PenUp() => IsPenDown = false;
    public void PenDown() => IsPenDown = true;

    // -- LUA METHODS -- //

    [MoonSharpVisible(true)] public void move(double d) => Move(d);
    [MoonSharpVisible(true)] public void teleport(double x, double y) => Teleport(x, y);
    [MoonSharpVisible(true)] public void center() => Teleport(0, 0);

    [MoonSharpVisible(true)] public void angle(double d) => SetAngle(d);
    [MoonSharpVisible(true)] public void turn(double d) => Turn(d);

    [MoonSharpVisible(true)] public void pen_size(double thickness) => SetPenSize(thickness);
    [MoonSharpVisible(true)] public void pen_up() => PenUp();
    [MoonSharpVisible(true)] public void pen_down() => PenDown();

    [MoonSharpVisible(true)] public void color(string? hex) => Color(hex);

}
