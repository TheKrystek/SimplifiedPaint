using SimplifiedPaintCore;
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

    public delegate void ButtonAddAction(Button button, IAbstractTool tool);

    public class ToolsContainer
    {
        private ButtonAddAction onButtonCreated;
        List<IAbstractTool> tools = new List<IAbstractTool>();

        public ToolsContainer()
        {
            // Load libraries and add them to the list
            PluginLoader.Instance.LoadLibraries();

            initializeTools();
        }

        public List<Button> GetButtons(Context context)
        {
            initializeTools();
            List<Button> buttons = new List<Button>();
            foreach (var tool in tools)
            {
                var button = getButton(tool, context);
                buttons.Add(button);
                onButtonCreatedAction(button, tool);
            }    
            return buttons;
        }

        private void initializeTools()
        {
            tools.Clear();

            // Add two basic tools
            tools.Add(new PenTool());
            tools.Add(new ColorPickerTool());

            getToolsFromPlugins();
        }

        private void getToolsFromPlugins()
        {
            foreach (var model in PluginLoader.Instance.EnabledPluginModels)
                tools.Add(model.Plugin.Tool);
        }


        private Button getButton(IAbstractTool tool, Context context)
        {
            IconButtonBuilder.Icon = tool.Icon;
            IconButtonBuilder.Name = tool.Name;
            IconButtonBuilder.Description = tool.GetDescription(LocalizeDictionary.CurrentCulture.Name);

            if (tool.Key.HasValue)
                IconButtonBuilder.Description += string.Format(" (ALT + {0})", tool.Key.Value.ToString());

            Button button = IconButtonBuilder.GetButton();

            button.Click += (s, e) =>
            {
                context.ChangeTool(tool);
                context.SelectedButton = button;
            };

            return button;
        }


        private void onButtonCreatedAction(Button button, IAbstractTool tool) {
            if (onButtonCreated != null)
                onButtonCreated(button, tool);
        }

        public ButtonAddAction OnButtonCreated
        {
            get
            {
                return onButtonCreated;
            }

            set
            {
                onButtonCreated = value;
            }
        }
    }
}
