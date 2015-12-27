using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimplifiedPaint
{
    class CanvasRenderer
    {

        public static void SaveToPNG(Canvas canvas, string filePath = "canvas.png")
        {
            Rect rect = new Rect(canvas.RenderSize);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            System.IO.File.WriteAllBytes(filePath, ms.ToArray());
        }

        public static ImageBrush LoadFromFile(string filePath) {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
            return brush;
        }
    }

}
