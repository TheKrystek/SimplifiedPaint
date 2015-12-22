using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    public class PluginsContainer : ObservableCollection<PluginModel>
    {

        public PluginsContainer(List<PluginModel> list) : base(list)
        {
        }



    }
}
