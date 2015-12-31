using SimplifiedPaintCore;
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

        #region Actions
        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(Context.Canvas);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                action(Context.ForeColorBrush, e);

            if (e.RightButton == MouseButtonState.Pressed)
                action(Context.BackColorBrush, e);
        }

        private void action(Brush brush, MouseEventArgs e)
        {
            Line line = new Line();
            line.Stroke = brush;
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

        public void OnMouseUp(MouseButtonEventArgs e)
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
            }
        }
        #endregion

        #region Description
        public string GetDescription(string langCode)
        {
            if (langCode.Contains("pl"))
                return "Rysuje dowolne kształty";
            return "Draws anything you wish";
        }

        public string Name
        {
            get
            {
                return "Pen";
            }
        }

        public string Icon
        {
            get
            {
                return "F1 M 21.5367,46.0076L 19,57L 29.3932,54.6016C 28.0768,50.6411 25.8696,47.0904 21.5367,46.0076 Z M 39,53L 69.4259,22.5741C 67.0871,17.8183 63.7005,13.6708 59.5673,10.4327L 31,39L 31,45L 39,45L 39,53 Z M 29,38L 57.8385,9.1615C 56.438,8.19625 54.9638,7.33038 53.4259,6.57407L 24,36L 24,38L 29,38 Z ";
            }
        }

        public Key? Key
        {
            get
            {
                return System.Windows.Input.Key.P;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
