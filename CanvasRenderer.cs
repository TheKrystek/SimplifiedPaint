using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimplifiedPaint
{
    class ImageHelper
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

            // Enforce no cache policy 
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.UriSource = new Uri(filePath);
            bmi.CacheOption = BitmapCacheOption.None;
            bmi.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmi.EndInit();
            brush.ImageSource = bmi;
            return brush;
        }


        public static Bitmap GetImageInfo(string filePath) {
            if (!new FileInfo(filePath).Exists)
                return null;
            return new Bitmap(filePath);
        }
            
    }

}
