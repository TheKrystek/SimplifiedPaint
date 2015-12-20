using SimplifiedPaint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFLocalizeExtension.Engine;

namespace InternetCrawlerGUI.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {

        #region 
        Context context;

        PluginLoader loader = new PluginLoader();

        #endregion


        public Main()
        {
            // Create new context
            context = new Context();

            // Initialize all components
            InitializeComponent();
            InitializeToolOptions();

            // Assign paintArea to context
            context.Canvas = paintArea;
            context.Status = currentTool;

            IToolPlugin plugin = new PenPlugin();

            Button button = loader.getButton(plugin, context);

            foreColorPicker.SelectedColor = Colors.Black;
            backColorPicker.SelectedColor = Colors.White;

         


            toolsPanel.Children.Add(button);

            paintArea.MouseDown += Canvas_MouseDown_1;
            paintArea.MouseMove += Canvas_MouseMove_1;
        }

 



        private void InitializeToolOptions()
        {
            context.ToolOptions.Add(colorOption);
            context.ToolOptions.Add(thicknessOption);
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (context.Tool != null)
                context.Tool.OnMouseDown(e);
        }


        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (context.Tool != null)
                context.Tool.OnMouseMove(e);
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


        #region Events handlers

        private void foreColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!foreColorPicker.SelectedColor.HasValue)
                return;

            context.ForeColor = foreColorPicker.SelectedColor.Value;
        }

        private void backColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!backColorPicker.SelectedColor.HasValue)
                return;
            context.BackColor = backColorPicker.SelectedColor.Value;
        }

        private void thicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            context.StrokeThickness = e.NewValue;
        }

        #endregion


    }
}
