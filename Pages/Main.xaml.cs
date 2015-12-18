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
        public Main()
        {
            InitializeComponent();


            paintArea.MouseDown += Canvas_MouseDown_1;
            paintArea.MouseMove += Canvas_MouseMove_1;

            paintArea.DefaultDrawingAttributes.Color = Colors.Purple;
            paintArea.MoveEnabled = true; 
        }

        int thickness = 10;
        Point currentPoint = new Point();

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
          
        }


        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thickness++;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LocalizeDictionary.Instance.Culture = new CultureInfo("pl");
        }
    }
}
