using SimplifiedPaintCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

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
        }

        private void loadLibraries()
        {

            DirectoryInfo d = new DirectoryInfo(pluginPath);
            if (!d.Exists)
                return;

            models.Clear();

            try
            {
                foreach (var file in d.GetFiles("*.dll"))
                    loadLibrary(file);
            }
            catch (Exception)
            {
                Debug.Print("No plugins found");
            }
           
        }

        private void loadLibrary(FileInfo path)
        {
            string assembly = Path.GetFullPath(path.FullName);
            Assembly ptrAssembly = Assembly.LoadFile(assembly);

            foreach (Type item in ptrAssembly.GetTypes())
            {
                if (!item.IsClass) continue;
                if (item.GetInterfaces().Contains(typeof(IToolPlugin))) {
                    var plugin = (IToolPlugin)Activator.CreateInstance(item);
                    Plugins.Add(plugin);
                    var model = new PluginModel(plugin);
                    model.File = path.Name;
                    models.Add(model);
                }
            }
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
