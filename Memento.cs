using SimplifiedPaintCore;
using System.Windows;

namespace SimplifiedPaint
{
    public class Memento
    {
        int start, end;
        UIElement[] items;
        double width, height;
        IAbstractTool tool;

        public Memento(int start, int end, UIElement[] items)
        {
            this.start = start;
            this.end = end;
            this.items = items;
        }

        public void setCanvasSize(double width, double height) {
            this.width = width;
            this.height = height;
        }

        public int Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public int End
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
            }
        }

        public UIElement[] Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public IAbstractTool Tool
        {
            get
            {
                return tool;
            }

            set
            {
                tool = value;
            }
        }

        public override string ToString()
        {
            return string.Format("({0};{1}> Items: {2}", start, end, items.Length);
        }

    }
}
