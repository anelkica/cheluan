using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace cheluan.Models;

public class TurtleRenderer
{
    private readonly Canvas _canvas;

    public TurtleRenderer(Canvas canvas)
    {
        _canvas = canvas;
    }

    public void Clear() 
    {
        void clearCanvas() => _canvas.Children.Clear();

        if (Dispatcher.UIThread.CheckAccess()) // already on UI thread? fire!
            clearCanvas();
        else
            Dispatcher.UIThread.Post(clearCanvas);
    }

    public async void DrawStep(TurtleStep step) // connected in MainWindow on turtle.OnMove
    {
        SolidColorBrush brush = new(Color.Parse(step.PenColorHex));

        // local method for easier refactoring
        void drawLine()
        {
            Line line = new Line
            {
                StartPoint = step.StartPoint,
                EndPoint = step.EndPoint,

                Stroke = brush,
                StrokeThickness = step.PenSize,

                StrokeJoin = PenLineJoin.Round, 
                StrokeLineCap = PenLineCap.Round
            };

            _canvas.Children.Add(line);
        }

        // https://docs.avaloniaui.net/docs/guides/development-guides/accessing-the-ui-thread
        // await Dispatcher.UIThread.InvokeAsync(drawLine); <-- Post is better bcuz we don't wait for a result (fire-and-forget)

        if (Dispatcher.UIThread.CheckAccess()) // already on UI thread? fire!
            drawLine();
        else
            Dispatcher.UIThread.Post(drawLine); 
    }

    public async void DrawFill(TurtleFill fill)
    {
        SolidColorBrush fillBrush = new(Color.Parse(fill.FillColorHex));
        SolidColorBrush strokeBrush = new(Color.Parse(fill.StrokeColorHex));

        void drawPolygon()
        {
            Polygon polygon = new()
            {
                Points = new Points(fill.Points),
                Fill = fillBrush,
                Stroke = strokeBrush,
                StrokeThickness = fill.StrokeThickness,
                StrokeJoin = PenLineJoin.Round
            };

            _canvas.Children.Insert(0, polygon);
        }

        if (Dispatcher.UIThread.CheckAccess())
            drawPolygon();
        else
            Dispatcher.UIThread.Post(drawPolygon);
    }
}
