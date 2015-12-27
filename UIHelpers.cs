using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimplifiedPaint
{
   public static class UIHelpers
    {

        public static UIElement[] GetRange(Canvas canvas, int start)
        {
            int size = canvas.Children.Count - start;
            UIElement[] items = new UIElement[size];
            for (int i = 0; i < size; i++)
                items[i] = canvas.Children[start + i];
            return items;
        }
    }
}
