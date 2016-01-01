using FirstFloor.ModernUI.Windows.Controls;
using SimplifiedPaint;
using SimplifiedPaintCore;
using SimplifiedPaint.Pages;
using SimplifiedPaint.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        SlidingWindowList<Memento> undoMechanism = new SlidingWindowList<Memento>(50);
        Context context;
        ToolsContainer toolsContainer = new ToolsContainer();
        string filePath = "";
        bool? saved;
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
            context.OnStatusChange += setStatus;
            context.ClearTool();

            // Set default colorpickers values
            foreColorPicker.SelectedColor = Colors.Black;
            backColorPicker.SelectedColor = Colors.White;
            context.OnForeColorChange += color => foreColorPicker.SelectedColor = color;
            context.OnBackColorChange += color => backColorPicker.SelectedColor = color;

            // Set up event handlers
            paintArea.MouseDown += Canvas_MouseDown;
            paintArea.MouseMove += Canvas_MouseMove;
            paintArea.MouseUp += Canvas_MouseUp;
            Application.Current.MainWindow.Closing += onClosing;

            addUndoRedoButtons();
            resizeCanvas(400, 400);
            Saved = null;

            addToolsHotKeys();
            addHotKeys();
        }

        private void addToolsHotKeys()
        {
            toolsContainer.OnButtonCreated += (button, tool) =>
            {
                if (tool.Key.HasValue)
                {
                    addHotKey(tool.Key.Value, (s, e) =>
                    {
                        context.ChangeTool(tool);
                    }, ModifierKeys.Alt);
                }
            };
        }

        #region Undo/Redo mechanism and other buttons from upperPanel  

        /// <summary>
        /// Setup Undo and Redo buttons and add them to panel
        /// </summary>
        private void addUndoRedoButtons()
        {
            string toolTip = string.Format(LocalizationProvider.GetLocalizedValue<string>("AppHistorySize"), undoMechanism.MaxSize);

            undo.Click += undoHandler;
            undo.IsEnabled = false;
            undo.ToolTip = toolTip;

            redo.Click += redoHandler;
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
            try
            {
                var memento = new Memento(prevCount, paintArea.Children.Count, UIHelpers.GetRange(paintArea, prevCount));
                memento.setCanvasSize(paintArea.ActualWidth, paintArea.ActualHeight);
                memento.Tool = context.Tool;
                undoMechanism.Add(memento);
            }
            catch (Exception)
            {
            }

            Saved = false;
        }

        private void Redo()
        {
            if (!redo.IsEnabled)
                return;

            context.Tool.OnRedo();

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

            resizeCanvas(nextState.Width, nextState.Height);
            context.ChangeTool(nextState.Tool);


            if (undoMechanism.isNull())
                redo.IsEnabled = false;
            undo.IsEnabled = true;
        }

        private void Undo()
        {
            if (undoMechanism.IsEmpty || !undo.IsEnabled)
                return;

            context.Tool.OnUndo();

            Memento prevState = undoMechanism.Get();
            undoMechanism.MoveCursorDown();
            paintArea.Children.RemoveRange(prevState.Start, prevState.Items.Length);
            resizeCanvas(prevState.Width, prevState.Height);
            context.ChangeTool(prevState.Tool);

            // If there is no more moves to undo
            if (undoMechanism.IsEmpty)
                undo.IsEnabled = false;
            redo.IsEnabled = true;
        }


        private void redoHandler(object sender, RoutedEventArgs e)
        {
            Redo();
        }

        private void undoHandler(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        private void resizeCanvas(double width, double height)
        {
            Console.WriteLine("Resized from {0}x{1} to {2}x{3}", canvasColumn.Width.Value, canvasRow.Height.Value, width, height);
            canvasColumn.Width = new GridLength(width);
            canvasRow.Height = new GridLength(height);
            setStatus();
        }

        private void discardChanges(object sender, RoutedEventArgs e)
        {
            // Show dialog and ask if user really wants to discard all changes
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
                clear();
        }

        private void clear()
        {
            paintArea.Children.Clear();
            paintArea.Background = Brushes.White;

            undoMechanism.Clear();
            undo.IsEnabled = false;
            redo.IsEnabled = false;
            context.ClearTool();

            Saved = null;
            prevCount = 0;
        }

        private void saveImage(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";

            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                saveImage(dlg.FileName);
                filePath = dlg.FileName;
            }
        }

        private void saveImage(string filePath)
        {
            resetProgressBar();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                // Fake delay - just for showcase purposes
                delayAndUpdateProgress(20);
                delayAndUpdateProgress(40);
                delayAndUpdateProgress(60);

                // Save image to file
                Dispatcher.Invoke(new Action(() => ImageHelper.SaveToPNG(paintArea, filePath)));
                delayAndUpdateProgress(99);
                delayAndUpdateProgress(100);

                Dispatcher.Invoke(new Action(() =>
                {
                    Saved = true;
                }));
            }).Start();
        }

        private void delayAndUpdateProgress(int progress)
        {
            Thread.Sleep(Settings.Default.Delay);
            Dispatcher.Invoke(new System.Action(() => progressBar.Value = progress));
        }

        private void saveImageQuiet(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                saveImage(sender, e);
            else
                saveImage(filePath);
        }

        private void openImage(object sender, RoutedEventArgs e)
        {
            // First of all check if there are unsaved changes
            checkForUnsavedChanges();

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";


            bool? result = dlg.ShowDialog();

            if (!result.HasValue || !result.Value)
                return;

            clear();
            filePath = dlg.FileName;
            // Create brush from file and resize canvas to proper dimension
            paintArea.Background = ImageHelper.LoadFromFile(dlg.FileName);
            System.Drawing.Bitmap img = ImageHelper.GetImageInfo(dlg.FileName);
            resizeCanvas(img.Width, img.Height);
            Saved = true;
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
            context.ClearButtons();
            foreach (var item in toolsContainer.GetButtons(context))
            {
                toolsPanel.Children.Add(item);
                context.AddButton(item);
            }
            context.UnselectButtons();
        }
        #endregion

        #region Events handlers

        int prevCount = 0;
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            context.Tool.OnMouseUp(e);

            SaveState();
        }

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (context.Tool == null)
                return;

            context.Tool.OnMouseDown(e);
            if (context.SaveState)
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

        public bool? Saved
        {
            get
            {
                return saved;
            }

            set
            {
                saved = value;
                save.IsEnabled = (value != null);
                discard.IsEnabled = (value != null);
                setStatus();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (count == 0)
                loadTools();
            count = (count + 1) % 2;
        }

        private void onClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            checkForUnsavedChanges();
        }
        #endregion

        #region Dialogs 
        private void checkForUnsavedChanges()
        {
            // First of all check if there are unsaved changes
            if (Saved.HasValue && !Saved.Value && askForSave() == FileDialogResults.yes)
                saveImage(null, null);
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
            dlg.Buttons = new Button[] { dlg.YesButton, dlg.NoButton, };
            dlg.ShowDialog();

            if (dlg.DialogResult.HasValue)
                if (dlg.DialogResult.Value)
                    return FileDialogResults.yes;
            return FileDialogResults.no;
        }

        public void setStatus()
        {
            string formatCurrentTool = LocalizationProvider.GetLocalizedValue<string>("CanvasCurrnetTool");
            string formatCanvasSize = LocalizationProvider.GetLocalizedValue<string>("CanvasSize");

            if (context.Tool != null)
                statusCurrentTool.Text = string.Format(formatCurrentTool, context.Tool.ToString());
            else
                statusCurrentTool.Text = "";

            statusCanvasSize.Text = string.Format(formatCanvasSize, canvasColumn.Width, canvasRow.Height);
            if (Saved.HasValue)
                statusSaved.Text = LocalizationProvider.GetLocalizedValue<string>(Saved.Value ? "CanvasIsSaved" : "CanvasIsUnsaved");
            else
                statusSaved.Text = LocalizationProvider.GetLocalizedValue<string>("CanvasIsNew");
        }

        private void resetProgressBar()
        {
            progressBar.Opacity = 1.0;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;

        }

        private void hideProgressBar()
        {
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
            };

            progressBar.BeginAnimation(OpacityProperty, animation);

        }

        private void canvasResized(object sender, SizeChangedEventArgs e)
        {
            setStatus();
        }

        private void openResizeDialog(object sender, RoutedEventArgs e)
        {
            ChangeSize dlg = new ChangeSize(canvasColumn.Width.Value, canvasRow.Height.Value);
            dlg.ShowDialog();

            resizeCanvas(dlg.NewWidth, dlg.NewHeight);
        }
        enum FileDialogResults
        {
            yes, no, cancel, ok
        }
        #endregion


        private void addHotKeys()
        {
            try
            {
                // CTRL + S -> Save
                addHotKey(Key.S, saveImageQuiet);

                // CTRL + Z -> Undo
                addHotKey(Key.Z, undoHandler);

                // CTRL + Y -> Redo
                addHotKey(Key.Y, redoHandler);

                // CTRL + O -> Open
                addHotKey(Key.O, openImage);

                // CTRL + R -> Resize
                addHotKey(Key.R, openResizeDialog);

                // CTRL + D -> Discard
                addHotKey(Key.D, discardChanges);

                // CTRL + '+' -> Thickness++
                addHotKey(Key.OemPlus, (s, e) => { if (thicknessOption.IsVisible) thicknessSlider.Value++; });
                addHotKey(Key.Add, (s, e) => { if (thicknessOption.IsVisible) thicknessSlider.Value++; });

                // CTRL + '-' -> Thickness--
                addHotKey(Key.OemMinus, (s, e) => { if (thicknessOption.IsVisible) thicknessSlider.Value--; });
                addHotKey(Key.Subtract, (s, e) => { if (thicknessOption.IsVisible) thicknessSlider.Value--; });


            }
            catch (Exception e)
            { }
        }

        private void addHotKey(Key key, ExecutedRoutedEventHandler handler, ModifierKeys modifier = ModifierKeys.Control)
        {
            RoutedCommand command = new RoutedCommand();
            command.InputGestures.Add(new KeyGesture(key, modifier));
            CommandBindings.Add(new CommandBinding(command, handler));
        }

        private void progressBarChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 100.0)
                progressBar.Visibility = Visibility.Hidden;
        }
    }
}
