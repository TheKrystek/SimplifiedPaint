using SimplifiedPaintCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimplifiedPaint
{
    class ColorPickerTool : IAbstractTool
    {
        Context context;

        #region Actions
        public void OnMouseDown(MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(context.Canvas);
            if (e.LeftButton == MouseButtonState.Pressed) {
                context.ForeColor = getAverageColor(p);
                context.ChangeTool(context.PrevTool);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                context.BackColor = getAverageColor(p);
                context.ChangeTool(context.PrevTool);
            }
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            // no action
        }

        private Color getAverageColor(Point point)
        {
            // Use RenderTargetBitmap to get the visual, in case the image has been transformed.
            var renderTargetBitmap = new RenderTargetBitmap((int)context.Canvas.ActualWidth,
                                                            (int)context.Canvas.ActualHeight,
                                                            96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(context.Canvas);

            // Make sure that the point is within the dimensions of the image.
            if ((point.X <= renderTargetBitmap.PixelWidth) && (point.Y <= renderTargetBitmap.PixelHeight))
            {
                // Create a cropped image at the supplied point coordinates.
                var croppedBitmap = new CroppedBitmap(renderTargetBitmap,
                                                      new Int32Rect((int)point.X, (int)point.Y, 1, 1));
                // Copy the sampled pixel to a byte array.
                var pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);

                // Assign the sampled color to a SolidColorBrush and return as conversion.
                return Color.FromArgb(255, pixels[2], pixels[1], pixels[0]);
            }
            return Colors.Black;
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            // no action
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
            return new List<Options>() { Options.COLORS };
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
                context.Canvas.Cursor = Cursors.Cross;
            }
        }
        #endregion

        #region Description
        public string GetDescription(string langCode)
        {
            if (langCode.Contains("pl"))
                return "Pobiera kolor";
            return "Picks a color";
        }

        public string Name
        {
            get
            {
                return "ColorPicker";
            }
        }

        public string Icon
        {
            get
            {
                return "M19.35,11.72L17.22,13.85L15.81,12.43L8.1,20.14L3.5,22L2,20.5L3.86,15.9L11.57,8.19L10.15,6.78L12.28,4.65L19.35,11.72M16.76,3C17.93,1.83 19.83,1.83 21,3C22.17,4.17 22.17,6.07 21,7.24L19.08,9.16L14.84,4.92L16.76,3M5.56,17.03L4.5,19.5L6.97,18.44L14.4,11L13,9.6L5.56,17.03Z";
            }
        }

        public Key? Key
        {
            get
            {
                return System.Windows.Input.Key.C;
            }
        }

        public override string ToString()
        {
            return Name;
        }       
        #endregion
    }
}
