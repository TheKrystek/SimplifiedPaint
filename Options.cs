using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    public sealed class Options
    {
        private readonly string name;
        private readonly int value;

        public static readonly Options COLORS = new Options(1, "colorOption");
        public static readonly Options THICKNESS = new Options(2, "thicknessOption");

        private Options(int value, String name)
        {
            this.name = name;
            this.value = value;
        }

        public override String ToString()
        {
            return name;
        }
  
    }
}
