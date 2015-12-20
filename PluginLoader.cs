using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFLocalizeExtension.Engine;

namespace SimplifiedPaint
{
    class PluginLoader
    {

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


    }
}
