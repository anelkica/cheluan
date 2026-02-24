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
    public void Move(double distance)
    {
        Point oldPos = Position;

        double dx = distance * Math.Cos(AngleRadians);
        double dy = -distance * Math.Sin(AngleRadians); // flip the Y because +Y means up but screen space is inverted

        Point newPos = new(Position.X + dx, Position.Y + dy);

        if (newPos.X < 0 || newPos.Y < 0 || newPos.X > Bounds.Width || newPos.Y > Bounds.Height)
            throw new Exception($"Turtle out of bounds: ({newPos.X:F1},{newPos.Y:F1})! Bounds: ({Bounds.Width},{Bounds.Height})");

        Position = newPos;

        if (IsPenDown)
            OnMove?.Invoke(new TurtleStep(oldPos, newPos, PenColor, PenSize));
    }

    public void Teleport(double localCoordinateX, double localCoordinateY)
    {
        // convert to absolute/canvas coordinates, bcuz local (0,0) means (200,200) in absolute cords (default canvas is 400x400)
        double canvasX = (Bounds.Width / 2) + localCoordinateX;
        double canvasY = (Bounds.Height / 2) - localCoordinateY; // invert Y so it goes up on the screen

        if (canvasX < 0 || canvasX > Bounds.Width || canvasY < 0 || canvasY > Bounds.Height)
            throw new Exception($"Position ({localCoordinateX}, {localCoordinateY}) is outside the visible area");

        Position = new Point(canvasX, canvasY);
    }

    public void Rect(double width, double height)
    {
        if (width <= 0 || height <= 0)
            throw new Exception($"Rectangle dimensions must be positive. Dimensions: ({width}x{height})");

        // literally the same code from python turtle.rect()
        for (int i = 0; i < 2; i++)
        {
            Move(width);
            Turn(90);
            Move(height);
            Turn(90);
        }
    }

    public void Circle(double radius) 
    {
        if (radius <= 0)
            throw new Exception($"Radius must be positive. Radius: {radius}");

        // i'll be honest, i don't know much maths but hope this works

        double circumference = 2 * Math.PI * radius;
        double segmentCount = Math.Max(12, (int)(circumference / 5)); // min 12 segments, 5px per segment
        double segmentAngle = 360 / segmentCount;
        double segmentDistance = circumference / segmentCount;

        for (int i = 0; i < segmentCount; i++) 
        {
            Move(segmentDistance);
            Turn(segmentAngle);
        }
    }

    public void Polygon(int sides, double size) 
    {
        if (sides < 3)
            throw new Exception($"Polygon must have at least 3 sides. Sides: {sides}");

        if (size <= 0)
            throw new Exception($"Polygon size must be positive. Size: {size}");

        double anglePerTurn = 360.0 / sides;
        for (int i = 0; i < sides; i++) 
        {
            Move(size);
            Turn(anglePerTurn);
        }
    }

    public void Color(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new Exception("Hex color is undefined");

        if (!hex.StartsWith("#")) // RRGGBB format maybe?
            hex = "#" + hex;

        if (hex.Length == 7 || hex.Length == 9) // #RRGGBB || #AARRGGBB
            PenColor = hex;
    }

    public void Color(double r, double g, double b, double a = -1) //
    {
        int validRed = Math.Clamp((int)r, 0, 255);
        int validGreen = Math.Clamp((int)g, 0, 255);
        int validBlue = Math.Clamp((int)g, 0, 255);

        if (a < 0)
            Color($"#{validRed:X2}{validGreen:X2}{validBlue:X2}");
        else
        {
            int validAlpha = Math.Clamp((int)a, 0, 255);
            Color($"#{validAlpha:X2}{validRed:X2}{validGreen:X2}{validBlue:X2}");
        }
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

    [MoonSharpVisible(true)] public void rect(double w, double h) => Rect(w, h);
    [MoonSharpVisible(true)] public void circle(double r) => Circle(r);
    [MoonSharpVisible(true)] public void polygon(int sides, double size) => Polygon(sides, size);

    [MoonSharpVisible(true)] public void angle(double d) => SetAngle(d);
    [MoonSharpVisible(true)] public void turn(double d) => Turn(d);

    [MoonSharpVisible(true)] public void pen_size(double thickness) => SetPenSize(thickness);
    [MoonSharpVisible(true)] public void pen_up() => PenUp();
    [MoonSharpVisible(true)] public void pen_down() => PenDown();

    [MoonSharpVisible(true)] public void color(string? hex) => Color(hex);
    [MoonSharpVisible(true)] public void color(double r, double g, double b, double a = -1) => Color(r, g, b, a);

}
