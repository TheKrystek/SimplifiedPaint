using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    public interface IToolPlugin
    {
        string Author { get; }
        string Version { get; }
        // Icon in data format. For example from http://modernuiicons.com/ -> value from Data=""
        IAbstractTool Tool { get; }
    }
}
