using Avalonia;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;

namespace cheluan.Models;

/*
    TODO:
        (turtle): color, penUp, penDown
        (canvas): bg, scale(?)
*/

// a record tracking the movement of the turtle from start to end of a line
public record TurtleStep(Point startPos, Point endPos);

[MoonSharpUserData]
public class Turtle
{
    public Point Position { get; set; }
    public Size Bounds { get; set; } // safety against crossing the available canvas
    public double Angle { get; set; }
    public double AngleRadians => Angle * Math.PI / 180;

    public Action<TurtleStep>? OnMove;

    public void Reset()
    {
        // centers the turtle
        Position = new(Bounds.Width / 2, Bounds.Height / 2);
        Angle = 0;
    }

    // move forwards
    public Result Move(double distance)
    {
        Point oldPos = Position;

        double dx = distance * Math.Cos(AngleRadians);
        double dy = distance * Math.Sin(AngleRadians);

        Point newPos = new(Position.X + dx, Position.Y + dy);

        if (newPos.X < 0 || newPos.X > Bounds.Width || newPos.Y < 0 || newPos.Y > Bounds.Height)
        {
            return Result.Fail($"Turtle out of bounds: ({newPos.X:F1},{newPos.Y:F1})! Bounds: ({Bounds.Width},{Bounds.Height})");
        }

        Position = newPos;
        OnMove?.Invoke(new TurtleStep(oldPos, newPos));

        return Result.Ok();
    }

    public void Turn(double degrees)
    {
        Angle += degrees;
    }

    [MoonSharpVisible(true)]
    public void move(double d) => Move(d);

    [MoonSharpVisible(true)]
    public void turn(double d) => Turn(d);
}
