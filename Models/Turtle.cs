using MoonSharp.Interpreter;
using System;

namespace cheluan.Models;

[MoonSharpUserData]
public class Turtle
{
    public double X { get; set; } 
    public double Y { get; set; }
    public double Angle { get; set; }

    // move forwards
    public Result Move(double distance)
    {
        X += distance * Math.Cos(Angle * Math.PI / 180);
        Y += distance * Math.Cos(Angle * Math.PI / 180);

        return Result.Ok();
    }

    public Result Turn(double degrees)
    {
        Angle += degrees;

        return Result.Ok();
    }
}
