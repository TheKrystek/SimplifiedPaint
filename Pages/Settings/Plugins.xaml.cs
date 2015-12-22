using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplifiedPaint.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Plugins.xaml
    /// </summary>
    public partial class Plugins : UserControl
    {

        PluginsContainer pluginsContainer;

        public Plugins()
        {
            InitializeComponent();


            PluginLoader.LoadLibraries();
            PluginLoader.ListPlugins();

            List<PluginModel> models = new List<PluginModel>();
            foreach (var item in PluginLoader.Plugins)
            {
                models.Add(new PluginModel(item));
            }

            PluginsContainer = new PluginsContainer(models);
            dataGrid.ItemsSource = PluginsContainer;


        }

        public PluginsContainer PluginsContainer
        {
            get
            {
                return pluginsContainer;
            }

            set
            {
                pluginsContainer = value;
            }
        }
    }
}
