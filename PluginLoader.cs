using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFLocalizeExtension.Engine;

namespace SimplifiedPaint
{
    public sealed class PluginLoader
    {
        static readonly PluginLoader _instance = new PluginLoader();
        static readonly List<PluginModel> models = new List<PluginModel>();
        static List<IToolPlugin> plugins = new List<IToolPlugin>();
        static string pluginPath = "plugins";

        PluginLoader()
        { }

        public void LoadLibraries()
        {
            plugins.Clear();
            loadLibraries();
            createPluginsModel();
        }

        private void loadLibraries()
        {
            DirectoryInfo d = new DirectoryInfo(pluginPath);
            if (!d.Exists)
                return;

            foreach (var file in d.GetFiles("*.dll"))
                loadLibrary(file.FullName);
        }

        private void loadLibrary(string path)
        {
            string assembly = Path.GetFullPath(path);
            Assembly ptrAssembly = Assembly.LoadFile(assembly);
            foreach (Type item in ptrAssembly.GetTypes())
            {
                if (!item.IsClass) continue;
                if (item.GetInterfaces().Contains(typeof(IToolPlugin)))
                {
                    Plugins.Add((IToolPlugin)Activator.CreateInstance(item));
                    return;
                }
            }
            throw new Exception("Invalid DLL, Interface not found!");
        }


        private void createPluginsModel()
        {
            models.Clear();
            foreach (var item in plugins)
                models.Add(new PluginModel(item));
        }

        #region Getters and setters
        public static PluginLoader Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<IToolPlugin> Plugins
        {
            get
            {
                return plugins;
            }

            set
            {
                plugins = value;
            }
        }

        public List<PluginModel> AllPluginModels
        {
            get
            {
                return models;
            }
        }

        public List<PluginModel> EnabledPluginModels
        {
            get
            {
                return models.Where(p => p.Enabled).ToList();
            }
        }
        #endregion

    }
}
