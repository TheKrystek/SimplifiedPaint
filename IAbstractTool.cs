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
        #region Actions
        void OnMouseDown(MouseButtonEventArgs e);
        void OnMouseUp(MouseButtonEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        #endregion

        #region Description
        string Name { get; }
        string GetDescription(string langCode);
        string Icon { get; }
        #endregion

        #region Options and context
        ICollection<Options> GetToolOptions();
        Context Context { get; set; }
        #endregion
    }
}
