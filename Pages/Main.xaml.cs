using FirstFloor.ModernUI.Windows.Controls;
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

        #region Fields
        SlidingWindowList<Memento> undoMechanism = new SlidingWindowList<Memento>();
        Context context;
        ToolsContainer toolsContainer = new ToolsContainer();
        string filePath = "image.png";
        bool saved = true;
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

            // Set default colorpickers values
            foreColorPicker.SelectedColor = Colors.Black;
            backColorPicker.SelectedColor = Colors.White;

            addUndoRedoButtons();

            discard.Click += Clear_Click;

            // Set up event handlers
            paintArea.MouseDown += Canvas_MouseDown;
            paintArea.MouseMove += Canvas_MouseMove;
            paintArea.MouseUp += Canvas_MouseUp;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ModernDialog
            {
                Title = LocalizationProvider.GetLocalizedValue<string>("AppDiscardMessageTitle"),
                Content = new TextBlock()
                {
                    Text = LocalizationProvider.GetLocalizedValue<string>("AppDiscardMessage")
                }
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            dlg.ShowDialog();

            if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
            {
                paintArea.Children.Clear();
                saved = true;
                undo.IsEnabled = false;
                redo.IsEnabled = false;
                undoMechanism.Clear();
            }
        }

        #region Undo/Redo mechanism    

        /// <summary>
        /// Setup Undo and Redo buttons and add them to panel
        /// </summary>
        private void addUndoRedoButtons()
        {
            string toolTip = string.Format(LocalizationProvider.GetLocalizedValue<string>("AppHistorySize"), undoMechanism.MaxSize);

            undo.Content = LocalizationProvider.GetLocalizedValue<string>("AppUndo");
            undo.Click += (s, e) => Undo();
            undo.IsEnabled = false;
            undo.ToolTip = toolTip;

            redo.Content = LocalizationProvider.GetLocalizedValue<string>("AppRedo");
            redo.Click += (s, e) => Redo();
            redo.IsEnabled = false;
            redo.ToolTip = toolTip;
        }

        /// <summary>
        /// Save program state every time users uses paint tools
        /// </summary>
        private void SaveState()
        {
            // Enable Undo and disable Redo button
            undo.IsEnabled = true;
            redo.IsEnabled = false;

            undoMechanism.clearRange();
            var memento = new Memento(prevCount, paintArea.Children.Count, UIHelpers.GetRange(paintArea, prevCount));
            memento.setCanvasSize(paintArea.ActualWidth, paintArea.ActualHeight);
            memento.Tool = context.Tool;

            undoMechanism.Add(memento);
            undoMechanism.Display();

            saved = false;
        }

        private void Redo()
        {
            undoMechanism.MoveCursorUp();
            Memento nextState = undoMechanism.Get();
            if (nextState == null)
                return;

            if (nextState.Items != null)
                foreach (var item in nextState.Items)
                {
                    paintArea.Children.Remove(item);
                    paintArea.Children.Add(item);
                }

            setCanvasSize(nextState.Width, nextState.Height);
            context.ChangeTool(nextState.Tool);
            undoMechanism.Display();

            if (undoMechanism.isNull())
                redo.IsEnabled = false;
            undo.IsEnabled = true;
        }




        private void Undo()
        {
            if (undoMechanism.IsEmpty)
                return;

            Memento prevState = undoMechanism.Get();
            undoMechanism.MoveCursorDown();
            paintArea.Children.RemoveRange(prevState.Start, prevState.Items.Length);
            setCanvasSize(prevState.Width, prevState.Height);
            context.ChangeTool(prevState.Tool);
            undoMechanism.Display();

            // If there is no more moves to undo
            if (undoMechanism.IsEmpty)
                undo.IsEnabled = false;
            redo.IsEnabled = true;
        }


        private void setCanvasSize(double width, double height)
        {
            Console.WriteLine("Resized from {0}x{1} to {2}x{3}", canvasColumn.Width.Value, canvasRow.Height.Value, width, height);
            canvasColumn.Width = new GridLength(width);
            canvasRow.Height = new GridLength(height);
        }

        #endregion

        #region Tools       
        private void InitializeToolOptions()
        {
            context.ToolOptions.Add(colorOption);
            context.ToolOptions.Add(thicknessOption);
        }

        private void loadTools()
        {
            toolsPanel.Children.Clear();
            foreach (var item in toolsContainer.GetButtons(context))
                toolsPanel.Children.Add(item);
        }
        #endregion

        #region Events handlers

        int prevCount = 0;
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            SaveState();
        }

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            context.Tool.OnMouseDown(e);
            prevCount = paintArea.Children.Count;
        }


        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";

            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                saved = true;
                CanvasRenderer.SaveToPNG(paintArea, dlg.FileName);
            }
        }


        private void open_Click(object sender, RoutedEventArgs e)
        {
            // First of all check if there are unsaved changes
            if (!saved) {
                if (askForSave() == FileDialogResults.ok)
                    save_Click(sender, null);
                else
                    saved = true;
            }

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";
            dlg.FileName = filePath;

            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
                paintArea.Background = CanvasRenderer.LoadFromFile(dlg.FileName);

        }



        private FileDialogResults askForSave()
        {
            var dlg = new ModernDialog
            {
                Title = LocalizationProvider.GetLocalizedValue<string>("AppSaveFileTitle"),
                Content = new TextBlock()
                {
                    Text = LocalizationProvider.GetLocalizedValue<string>("AppSaveFileQuestion")
                }
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton,};
            

            if (dlg.DialogResult.HasValue)
                if (dlg.DialogResult.Value)
                    return FileDialogResults.ok;
            return FileDialogResults.cancel;
        }



        enum FileDialogResults {
            yes, no, cancel, ok
        }
    }
}
