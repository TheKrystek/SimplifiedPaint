using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplifiedPaint
{
    public interface IAbstractTool
    {
        ICollection<Options> GetToolOptions(); 
        void OnMouseDown(MouseButtonEventArgs e);
        void OnMouseUp(MouseButtonEventArgs e);
        void OnMouseMove(MouseEventArgs e);

        Context Context { get; set; }

    }
}
