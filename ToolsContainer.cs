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
   public class ToolsContainer
    {
        PluginsContainer pluginsContainer;

        List<IAbstractTool> tools = new List<IAbstractTool>();

        public ToolsContainer() {
            // Load libraries and add them to the list
            PluginLoader.Instance.LoadLibraries();

            initializeTools();
        }

        public List<Button> GetButtons(Context context) {
            initializeTools();
            List<Button> buttons = new List<Button>();
            foreach (var item in tools)
                buttons.Add(getButton(item, context));
            return buttons;
        }


        private void initializeTools() {
            tools.Clear();

            // Add one basic tool
            tools.Add(new PenTool());

            getToolsFromPlugins();
        }

        private void getToolsFromPlugins() {
            foreach (var model in PluginLoader.Instance.EnabledPluginModels)
                tools.Add(model.Plugin.Tool);
        }


        private Button getButton(IAbstractTool tool, Context context)
        {
            IconButtonBuilder.Icon = tool.Icon;
            IconButtonBuilder.Name = tool.Name;
            IconButtonBuilder.Description = tool.GetDescription(LocalizeDictionary.CurrentCulture.Name);
            Button button = IconButtonBuilder.GetButton();

            button.Click += (s, e) =>
            {
                context.ChangeTool(tool);
            };

            return button;
        }

    }
}
