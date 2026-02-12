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
        Y += distance * Math.Sin(Angle * Math.PI / 180);

        OnMove?.Invoke(new TurtleStep(oldX, oldY, X, Y));

        return Result.Ok();
    }

    public void Turn(double degrees)
    {
        Angle += degrees;
    }

    public void Reset(double x, double y) 
    {
        X = x;
        Y = y;
        Angle = 0;
        
    }

    [MoonSharpVisible(true)]
    public void move(double d) => Move(d);

    [MoonSharpVisible(true)]
    public void turn(double d) => Turn(d);
}
