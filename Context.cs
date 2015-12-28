using System;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace SimplifiedPaint
{
    public class Context
    {

        Color foreColor, backColor;
        Brush foreColorBrush, backColorBrush;
        double strokeThickness;
        Canvas canvas;
        IAbstractTool tool;
        public delegate void StatusChange();
        StatusChange onStatusChange;

        List<Panel> toolOptions = new List<Panel>();


        public Context(Canvas canvas) {
            this.Canvas = canvas;
        }

        public Context()
        {
        }

        public void showHideToolOption(string optionName, bool show = true) {
            Panel panel = ToolOptions.Find(p => p.Name.ToLowerInvariant().Contains(optionName.ToLowerInvariant()));

            if (panel != null)
                panel.Visibility = (show ? Visibility.Visible : Visibility.Collapsed);
        }

        public void ChangeTool(IAbstractTool newTool)
        {
            // Load tool only once
            if (tool != null && (tool.GetType() == newTool.GetType()))
                return;

            // Change tool
            tool = newTool;
            tool.Context = this;

            // Hide all options and show only those which are required by newTool
            hideAllOptions();
            foreach (var option in tool.GetToolOptions())
                showToolOption(option.ToString());

            onStatusChangeAction();
        }

        public void ClearTool()
        {
            // Change tool to null
            tool = null;

            // Hide all options and show only those which are required by newTool
            hideAllOptions();
            onStatusChangeAction();
        }


        public void showToolOption(string optionName) {
            this.showHideToolOption(optionName);
        }
        public void hideToolOption(string optionName)
        {
            this.showHideToolOption(optionName, false);
        }

        public void hideAllOptions() {
            foreach (var option in toolOptions)
                option.Visibility = Visibility.Collapsed;
        }


        public Color ForeColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                foreColor = value;
                foreColorBrush = new SolidColorBrush(value);
            }
        }

        public Brush ForeColorBrush
        {
            get
            {
                return foreColorBrush;
            }

            set
            {
                foreColorBrush = value;
            }
        }

        public Brush BackColorBrush
        {
            get
            {
                return backColorBrush;
            }

            set
            {
                backColorBrush = value;
            }
        }


        public Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
                backColorBrush = new SolidColorBrush(value);
            }
        }

        public double StrokeThickness
        {
            get
            {
                return strokeThickness;
            }

            set
            {
                strokeThickness = value;
            }
        }

        public List<Panel> ToolOptions
        {
            get
            {
                return toolOptions;
            }

            set
            {
                toolOptions = value;
            }
        }

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }

            set
            {
                canvas = value;
            }
        }

        public IAbstractTool Tool
        {
            get
            {
                return tool;
            }

            set
            {
                tool = value;
            }
        }

        public StatusChange OnStatusChange
        {
            get
            {
                return onStatusChange;
            }

            set
            {
                onStatusChange = value;
            }
        }

        protected void onStatusChangeAction()
        {
            if (onStatusChange != null)
                onStatusChange();
        }

    }
}
