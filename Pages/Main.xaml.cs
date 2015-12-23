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
        SlidingWindowList<Memento> undoMechanism = new SlidingWindowList<Memento>();

        #region 
        Context context;

        ToolsContainer toolsContainer = new ToolsContainer();

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

            foreColorPicker.SelectedColor = Colors.Black;
            backColorPicker.SelectedColor = Colors.White;

            paintArea.MouseDown += Canvas_MouseDown_1;
            paintArea.MouseMove += Canvas_MouseMove_1;
            paintArea.MouseUp += Canvas_MouseUp;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            SaveState();
        }

        private void loadTools()
        {
            toolsPanel.Children.Clear();
            foreach (var item in toolsContainer.GetButtons(context))
                toolsPanel.Children.Add(item);

            // TODO: zrobic obsluge cofania
            Button back = new Button();
            back.Content = "Undo";
            back.Click += (s, e) => Undo();


            // TODO: zrobic obsluge cofania
            Button redo = new Button();
            redo.Content = "Redo";
            redo.Click += (s, e) => Redo();

            toolsPanel.Children.Add(back);
            toolsPanel.Children.Add(redo);

        }

        private void Redo()
        {
            undoMechanism.MoveCursorUp();
            Memento nextState = undoMechanism.Get();
            if (nextState == null)
                return;

            foreach (var item in nextState.Items)
            {
                paintArea.Children.Remove(item);
                paintArea.Children.Add(item);
            }

            undoMechanism.Display();

        }

        private void SaveState()
        {
            undoMechanism.Add(new Memento(prevCount, paintArea.Children.Count, getRange(prevCount)));
            undoMechanism.Display();
        }


        private void Undo()
        {
            if (undoMechanism.IsEmpty)
                return;

            Memento prevState = undoMechanism.Get();
            undoMechanism.MoveCursorDown();
            paintArea.Children.RemoveRange(prevState.Start, prevState.Items.Length);

            undoMechanism.Display();
        }

        private UIElement[] getRange(int start)
        {
            int size = paintArea.Children.Count - start;
            UIElement[] items = new UIElement[size];
            for (int i = 0; i < size; i++)
                items[i] = paintArea.Children[start + i];

            return items;
        }




        private void InitializeToolOptions()
        {
            context.ToolOptions.Add(colorOption);
            context.ToolOptions.Add(thicknessOption);
        }

        #region Events handlers


        int prevCount = 0;

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            context.Tool.OnMouseDown(e);
            prevCount = paintArea.Children.Count;
        }


        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (context.Tool == null)
                return;
            context.Tool.OnMouseMove(e);
        }

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

        int count = 0;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (count == 0)
                loadTools();
            count = (count + 1) % 2;
        }
        #endregion


    }
}
