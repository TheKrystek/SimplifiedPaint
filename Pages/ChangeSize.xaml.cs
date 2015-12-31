using FirstFloor.ModernUI.Windows.Controls;
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

namespace SimplifiedPaint.Pages
{
    /// <summary>
    /// Interaction logic for ChangeSize.xaml
    /// </summary>
    public partial class ChangeSize : ModernDialog
    {
      
        double currentWidth, currentHeight, proportions, newWidth, newHeight;

        public double NewWidth
        {
            get
            {
                return newWidth;
            }

            set
            {
                newWidth = Math.Round(value);
            }
        }

        public double NewHeight
        {
            get
            {
                return newHeight;
            }

            set
            {

                newHeight = Math.Round(value);
            }
        }

        private void lockedChecked(object sender, RoutedEventArgs e)
        {
            if (!locked.IsChecked.HasValue || !locked.IsChecked.Value)
            {
                proportions = 0;
                return;
            }

            double w = getValue(widthTextBox);
            double h = getValue(heightTextBox);

            proportions = w / h;

            Console.WriteLine("Proportions: {0}", proportions);
        }

        private void widthTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NewWidth = getValue(widthTextBox);
                if (locked.IsChecked.HasValue && locked.IsChecked.Value)
                {
                    NewHeight = NewWidth / proportions;
                    heightTextBox.Text = NewHeight.ToString();
                }
            }
        }

        private void heightTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ) {
                NewHeight = getValue(heightTextBox);
                if (locked.IsChecked.HasValue && locked.IsChecked.Value)
                {
                    NewWidth = NewHeight * proportions;
                    widthTextBox.Text = NewWidth.ToString();
                }
            }
        }

        public ChangeSize()
        {
            InitializeComponent();

            Button closeButton = new Button()
            {
                Content = LocalizationProvider.GetLocalizedValue<string>("AppCancel")              
            };
            closeButton.Click += (s, e) => {
                NewHeight = currentHeight;
                NewWidth = currentWidth;
                Close();
            };


            Button okButton = new Button()
            {
                Content = LocalizationProvider.GetLocalizedValue<string>("AppOk")
            };
            okButton.Click += (s, e) => Close();


            Buttons = new Button[] { closeButton, okButton };
        }

        public ChangeSize(double width, double height) : this()
        {
            currentHeight = height;
            currentWidth = width;
            NewHeight = height;
            NewWidth = width;


            widthTextBox.Text = width.ToString();
            heightTextBox.Text = height.ToString();

            locked.IsChecked = true;
        }

        private double getValue(TextBox tb)
        {
            try
            {
                return double.Parse(tb.Text);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }



    }
}
