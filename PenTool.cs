using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimplifiedPaint
{
    class PenTool : IAbstractTool
    {
        Context context;
        private Point currentPoint;

        public ICollection<Options> GetToolOptions()
        {
            return new List<Options>() { Options.COLORS, Options.THICKNESS };
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(Context.Canvas);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                line.Stroke = Context.ForeColorBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(Context.Canvas).X;
                line.Y2 = e.GetPosition(Context.Canvas).Y;
                line.StrokeThickness = Context.StrokeThickness;
                line.StrokeStartLineCap = PenLineCap.Round;
                line.StrokeEndLineCap = PenLineCap.Round;

                currentPoint = e.GetPosition(Context.Canvas);

                Context.Canvas.Children.Add(line);
            }
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            // no action
        }

        public Context Context
        {
            get
            {
                return context;
            }

            set
            {
                context = value;
            }
        }


        public override string ToString()
        {
            return "pen";
        }
    }
}
