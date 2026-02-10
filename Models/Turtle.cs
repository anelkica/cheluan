using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;

namespace cheluan.Models;

// a record tracking the movement of the turtle from start to end of a line
public record TurtleStep(double StartX, double StartY, double EndX, double EndY);

[MoonSharpUserData]
public class Turtle
{
    public double X { get; set; } 
    public double Y { get; set; }
    public double Angle { get; set; }

    public Action<TurtleStep>? OnMove;

    // move forwards
    public Result Move(double distance)
    {
        double oldX = X;
        double oldY = Y;

        X += distance * Math.Cos(Angle * Math.PI / 180);
        Y += distance * Math.Cos(Angle * Math.PI / 180);

        OnMove?.Invoke(new TurtleStep(oldX, oldY, X, Y));

        return Result.Ok();
    }

    public Result Turn(double degrees)
    {
        Angle += degrees;

        return Result.Ok();
    }

    [MoonSharpVisible(true)]
    public Result move(double d) => Move(d);
}
