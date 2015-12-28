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

        /// <summary>
        /// Get the range of canvas elements
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="start"></param>
        /// <returns></returns>
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
