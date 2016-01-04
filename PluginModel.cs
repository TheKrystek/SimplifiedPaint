using SimplifiedPaintCore;
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
        private bool enabled = false;
        Geometry icon;
        private string file;

        public PluginModel(IToolPlugin plugin)
        {
            Plugin = plugin;
            IconButtonBuilder.Icon = plugin.Tool.Icon;
            icon = IconButtonBuilder.GetGeomtry();

            if (Properties.Settings.Default.EnabledPlugins == null)
                Properties.Settings.Default.EnabledPlugins = new System.Collections.Specialized.StringCollection();

            enabled = Properties.Settings.Default.EnabledPlugins.Contains(Name);
        }

        public string Name { get { return Plugin.Tool.Name; } }
        public string Author { get { return Plugin.Author; } }
        public string Version { get { return Plugin.Version; } }
        public string Description { get { return Plugin.Tool.GetDescription(LocalizeDictionary.Instance.Culture.Name); } }



        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                // Add or remove current plugin form list of enabled plugins
                if (value)
                    Properties.Settings.Default.EnabledPlugins.Add(Name);
                else
                    Properties.Settings.Default.EnabledPlugins.Remove(Name);
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

        public IToolPlugin Plugin
        {
            get
            {
                return plugin;
            }

            set
            {
                plugin = value;
            }
        }

        public string File
        {
            get
            {
                return file;
            }

            set
            {
                file = value;
            }
        }
    }
}
