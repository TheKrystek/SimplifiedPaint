using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    public interface IToolPlugin
    {
        string Name { get;}
        string Author { get; }
        string Version { get; }
        string GetDescription(string langCode);

        // Icon in data format. For example from http://modernuiicons.com/ -> value from Data=""
        string Icon { get; }
        IAbstractTool Tool { get; }

    }
}
