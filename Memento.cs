using System.Windows;
using System.Windows.Controls;

namespace SimplifiedPaint
{
    public class Memento
    {
        int start, end;
        UIElement[] items;


        public Memento(int start, int end, UIElement[] items)
        {
            this.start = start;
            this.end = end;
            this.items = items;
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

        public override string ToString()
        {
            return string.Format("({0};{1}> Items: {2}", start, end, items.Length);
        }

    }
}
