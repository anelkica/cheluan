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

    public double Thickness { get; set; } = 2;

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
        SolidColorBrush brush = new(Color.Parse(step.penColorHex));

        // local method for easier refactoring
        void drawLine()
        {
            Line line = new Line
            {
                StartPoint = step.startPos,
                EndPoint = step.endPos,

                Stroke = brush,
                StrokeThickness = step.penSize,

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


}
