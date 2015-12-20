using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimplifiedPaint
{
    class IconButtonBuilder
    {

        static string defaultIcon = "F1 M 29,18L 52.25,41.1667L 43.0865,42.6585L 50.817,56.6949L 43.827,60.4115L 36,46.25L 29,53.25L 29,18 Z ";
        static string icon = "";
        static string name = "";
        static string description = "";
        static Brush brush = Brushes.Black;

        public static Button GetButton(int width = 25, int height = 25)
        {
            return GetButton(width,height, new Thickness(5));
        }

        public static Button GetButton(int width, int height, Thickness padding)
        {
            Path path = createPath(width, height);
            Canvas canvas = createCanvas(path);

            Button button = new Button();
            button.Content = canvas;
            button.Padding = padding;

            setToolTip(button);
            return button;
        }

        private static void setToolTip(Button button)
        {
            string toolTip = "";
            if (!string.IsNullOrWhiteSpace(name))
                toolTip += name;

            if (!string.IsNullOrWhiteSpace(toolTip))
                toolTip += " - ";

            toolTip += description;

            button.ToolTip = toolTip;
        }

        private static Canvas createCanvas(Path path)
        {
            Canvas canvas = new Canvas();
            canvas.Children.Add(path);
            canvas.Width = path.Width;
            canvas.Height = path.Height;
            canvas.Clip = Geometry.Parse("F1 M 0, 0L 76, 0L 76, 76L 0, 76L 0, 0");
            return canvas;
        }

        private static Path createPath(int width, int height)
        {
            Path path = new Path();

            if (!string.IsNullOrWhiteSpace(icon)) {
                try
                {
                    path.Data = Geometry.Parse(icon);
                }
                catch (Exception)
                {
                    path.Data = Geometry.Parse(defaultIcon);
                }
            }
            else
                path.Data = Geometry.Parse(defaultIcon);


            path.Stretch = Stretch.Fill;
            path.Fill = brush;
            path.Width = width;
            path.Height = height;
            return path;
        }

        public static string Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
            }
        }

        public static string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public static string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public static Brush Brush
        {
            get
            {
                return brush;
            }

            set
            {
                brush = value;
            }
        }
    }
}
