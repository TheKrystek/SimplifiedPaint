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
    class RectangleTool : IAbstractTool
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

            Rectangle rect = new Rectangle();
            rect.Stroke = forecolor;
            rect.Fill = backcolor;
            rect.Width = Math.Abs(A.X - B.X);
            rect.Height = Math.Abs(A.Y - B.Y);

            rect.StrokeThickness = Context.StrokeThickness;
            rect.StrokeStartLineCap = PenLineCap.Round;
            rect.StrokeEndLineCap = PenLineCap.Round;

            Context.Canvas.Children.Add(rect);

            Point padding = GeometryUtil.Min(A, B);
            Canvas.SetLeft(rect, padding.X);
            Canvas.SetTop(rect, padding.Y);
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (mouseKey == MouseAction.LeftClick)
                action(Context.ForeColorBrush, Context.BackColorBrush, e);
            else
                action(Context.BackColorBrush, Context.ForeColorBrush, e);

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
            }
        }
        #endregion

        #region Description
        public string GetDescription(string langCode)
        {
            if (langCode.Contains("pl"))
                return "Rysuje prostokąty i kwadraty";
            return "Draws rectangles and squares";
        }

        public string Name
        {
            get
            {
                return "Rectangle";
            }
        }

        public string Icon
        {
            get
            {
                return "F1 M 23,28L 23,29L 22,29L 22,47L 23,47L 23,48L 53,48L 53,47L 54,47L 54,29L 53,29L 53,28L 23,28 Z M 58,47L 58,52L 53,52L 53,51L 23,51L 23,52L 18,52L 18,47L 19,47L 19,29L 18,29L 18,24L 23,24L 23,25L 53,25L 53,24L 58,24L 58,29L 57,29L 57,47L 58,47 Z M 19,25L 19,28L 22,28L 22,25L 19,25 Z M 54,25L 54,28L 57,28L 57,25L 54,25 Z M 19,48L 19,51L 22,51L 22,48L 19,48 Z M 54,48L 54,51L 57,51L 57,48L 54,48 Z ";
            }
        }

        public Key? Key
        {
            get
            {
                return System.Windows.Input.Key.R;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
