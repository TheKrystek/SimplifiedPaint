using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFLocalizeExtension.Engine;

namespace SimplifiedPaint
{
    public class PluginModel
    {
        private IToolPlugin plugin;

        Geometry icon;

        public PluginModel(IToolPlugin plugin)
        {
            this.plugin = plugin;

            IconButtonBuilder.Icon = plugin.Icon;
            icon = IconButtonBuilder.GetGeomtry();

        }

        private bool enabled;

        public string Name { get { return plugin.Name; } }


        public string Author { get { return plugin.Author; } }
        public string Version { get { return plugin.Version; } }
        public string Description { get { return plugin.GetDescription(LocalizeDictionary.Instance.Culture.Name); } }


        
        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        public Geometry Icon
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
    }
}
