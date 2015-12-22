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
    class PluginLoader
    {
        private static List<IToolPlugin> plugins = new List<IToolPlugin>();
        private static string pluginPath = "plugins";

        public static List<IToolPlugin> Plugins
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

        public Button getButton(IToolPlugin plugin, Context context)
        {
            IconButtonBuilder.Icon = plugin.Icon;
            IconButtonBuilder.Name = plugin.Name;
            IconButtonBuilder.Description = plugin.GetDescription(LocalizeDictionary.CurrentCulture.Name);

            Button button = IconButtonBuilder.GetButton();


            button.Click += (s, e) =>
            {
                Debug.Print("'{0}' has been choosed", plugin.Name);
                context.ChangeTool(plugin.Tool);
            };

            return button;
        }

        public static void LoadLibraries()
        {
            
            DirectoryInfo d = new DirectoryInfo(pluginPath);
            if (!d.Exists)
                return;

            foreach (var file in d.GetFiles("*.dll"))
            {
                Console.WriteLine(file.Name + " loading...");
                loadLibrary(file.FullName);
            }
        }


        static void loadLibrary(string path)
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


        public static void ListPlugins()
        {
            int i = 1;
            Console.WriteLine("\r\nAvailable plugins:");
            foreach (IToolPlugin plugin in Plugins)
            {
                Console.WriteLine("{0}. {1} - {2}", i++, plugin.Name, plugin.GetDescription("pl"));
            }
        }

    }
}
