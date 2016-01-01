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
    class EllipseTool : IAbstractTool
    {
        Context context;
        private Point A, B;
        MouseAction mouseKey;


        #region Actions
        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                A = e.GetPosition(Context.Canvas);

            mouseKey = e.LeftButton == MouseButtonState.Pressed ? MouseAction.LeftClick : MouseAction.RightClick;
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                B = e.GetPosition(Context.Canvas);
        }

        private void action(Brush forecolor, Brush backcolor, MouseEventArgs e)
        {
            if (A == null || B == null)
                return;

            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = forecolor;
            ellipse.Fill = backcolor;
            ellipse.Width = Math.Abs(A.X - B.X);
            ellipse.Height = Math.Abs(A.Y - B.Y);


            ellipse.StrokeThickness = Context.StrokeThickness;
            ellipse.StrokeStartLineCap = PenLineCap.Round;
            ellipse.StrokeEndLineCap = PenLineCap.Round;

            Context.Canvas.Children.Add(ellipse);

            Point padding = GeometryUtil.Min(A, B);
            Canvas.SetLeft(ellipse, padding.X);
            Canvas.SetTop(ellipse, padding.Y);

        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (mouseKey == MouseAction.LeftClick)
                action(Context.ForeColorBrush, Context.BackColorBrush, e);
            else
                action(Context.BackColorBrush, Context.ForeColorBrush, e);
        }


        public void OnUndo()
        {
            // no action
        }

        public void OnRedo()
        {
            // no action
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
                return "Rysuje elipsy i okręgi";
            return "Draws ellipses and circles";
        }

        public string Name
        {
            get
            {
                return "Ellipse";
            }
        }

        public string Icon
        {
            get
            {
                return "F1 M 38,36C 36.8954,36 36,36.8954 36,38C 36,39.1046 36.8954,40 38,40C 39.1046,40 40,39.1046 40,38C 40,36.8954 39.1046,36 38,36 Z M 38,34C 40.2091,34 42,35.7909 42,38C 42,40.2091 40.2091,42 38,42C 35.7909,42 34,40.2091 34,38C 34,35.7909 35.7909,34 38,34 Z M 57.75,36C 56.6454,36 55.75,36.8954 55.75,38C 55.75,39.1046 56.6454,40 57.75,40C 58.8546,40 59.75,39.1046 59.75,38C 59.75,36.8954 58.8546,36 57.75,36 Z M 61.75,38C 61.75,39.9038 60.4199,41.497 58.6383,41.901C 56.8098,51.635 48.265,59 38,59C 26.402,59 17,49.598 17,38C 17,26.402 26.402,17 38,17C 48.265,17 56.8098,24.365 58.6383,34.099C 60.4199,34.503 61.75,36.0962 61.75,38 Z M 53.75,38C 53.75,36.5505 54.521,35.281 55.6754,34.5794C 54.0776,26.2741 46.7715,20 38,20C 28.0589,20 20,28.0589 20,38C 20,47.9411 28.0589,56 38,56C 46.7715,56 54.0776,49.7259 55.6754,41.4206C 54.521,40.719 53.75,39.4496 53.75,38 Z ";
            }
        }

        public Key? Key
        {
            get
            {
                return System.Windows.Input.Key.E;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
