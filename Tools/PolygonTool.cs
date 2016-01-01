using SimplifiedPaintCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimplifiedPaint
{
    class PolygonTool : IAbstractTool
    {
        Context context;
        private Point A;
        MouseAction mouseKey;

        double precision = 6;
        int start = 0;

        PointCollection points;

        #region Actions
        public void OnMouseDown(MouseButtonEventArgs e)
        {
            mouseKey = e.LeftButton == MouseButtonState.Pressed ? MouseAction.LeftClick : MouseAction.RightClick;
            context.SaveState = false;
            if (points == null)
            {
                start = context.Canvas.Children.Count;
                points = new PointCollection();
            }


            if (mouseKey == MouseAction.LeftClick)
            {
                A = e.GetPosition(Context.Canvas);
                addCircle(A);

                // If we have at least three points then, check if last point can be used for closing shape 
                if (points.Count > 2 && GeometryUtil.Distance(points[0], A) <= precision)
                    close();
                else
                    points.Add(A);
            }

            if (mouseKey == MouseAction.RightClick)
                close();
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            // not used
        }

        private void close()
        {
            context.SaveState = true;

            Polygon poly = new Polygon();
            poly.Stroke = context.ForeColorBrush;
            poly.Fill = context.BackColorBrush;
            poly.StrokeThickness = Context.StrokeThickness;
            poly.StrokeStartLineCap = PenLineCap.Round;
            poly.StrokeEndLineCap = PenLineCap.Round;
            poly.Points = points;

            Context.Canvas.Children.Add(poly);

          
            clearTempPoints();
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (points == null || points.Count < 2)
                return;

            Point P = points[points.Count - 2];
            Line line = new Line();
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Black;
            line.X1 = P.X;
            line.Y1 = P.Y;
            line.X2 = A.X;
            line.Y2 = A.Y;
            context.Canvas.Children.Add(line);   
        }

        private void addCircle(Point point)
        {
            Ellipse circle = new Ellipse();
            circle.Width = precision;
            circle.Height = precision;
            circle.Fill = Brushes.Red;
            circle.Stroke = Brushes.Black;
            Canvas.SetZIndex(circle, 2);
            context.Canvas.Children.Add(circle);
            Canvas.SetTop(circle, point.Y - precision / 2);
            Canvas.SetLeft(circle, point.X - precision / 2);
        }

        private void clearTempPoints()
        {
            context.Canvas.Children.RemoveRange(start, context.Canvas.Children.Count - start -1);
            points = null;
        }

        public void OnUndo()
        {
            points = null;
        }

        public void OnRedo()
        {
            points = null;
        }
        #endregion

        #region Options and context
        public ICollection<Options> GetToolOptions()
        {
            return new List<Options>() { Options.COLORS, Options.THICKNESS };
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
                context.Canvas.Cursor = Cursors.Arrow;
            }
        }
        #endregion

        #region Description
        public string GetDescription(string langCode)
        {
            if (langCode.Contains("pl"))
                return "Rysuje wielokąt, pierwszy i ostatni punkt muszą być takie same. Prawy przycisk myszy zamyka figurę.";
            return " Draws a polygon, which is a connected series of lines that form a closed shape. Right mouse click closes shape.";
        }

        public string Name
        {
            get
            {
                return "Polygon";
            }
        }

        public string Icon
        {
            get
            {
                return "F1 M 33,20L 33,23L 58.5,51L 60,51L 60,56L 55,56L 55,54.5L 23,44.5L 23,45L 18,45L 18,40L 20,40L 28,23L 28,20L 33,20 Z M 31,25L 30,25L 23,40L 23,41.5L 55,51.5L 55,51L 31,25 Z M 56,55L 59,55L 59,52L 56,52L 56,55 Z M 29,24L 32,24L 32,21L 29,21L 29,24 Z M 22,44L 22,41L 19,41L 19,44L 22,44 Z ";
            }
        }

        public Key? Key
        {
            get
            {
                return null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
