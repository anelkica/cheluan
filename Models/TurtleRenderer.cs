using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;

namespace cheluan.Models;

public class TurtleRenderer
{
    private readonly Canvas _canvas;

    public IBrush LineBrush { get; set; } = Brushes.LawnGreen;
    public double Thickness { get; set; } = 2;

    public TurtleRenderer(Canvas canvas)
    {
        _canvas = canvas;
    }

    public void Clear() => Dispatcher.UIThread.Post(() => _canvas.Children.Clear());

    public void DrawStep(TurtleStep step)
    {
        Point start = new Point(step.StartX, step.StartY);
        Point end = new Point(step.EndX, step.EndY);

        Dispatcher.UIThread.Post(() =>
        {
            Line line = new Line
            {
                StartPoint = start,
                EndPoint = end,

                Stroke = LineBrush,
                StrokeThickness = Thickness,

                StrokeJoin = PenLineJoin.Round,
                StrokeLineCap = PenLineCap.Round
            };

            _canvas.Children.Add(line);
        });
    }


}
