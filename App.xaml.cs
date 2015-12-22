using FirstFloor.ModernUI.Presentation;
using SimplifiedPaint.Properties;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using WPFLocalizeExtension.Engine;

namespace SimplifiedPaint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitialiseCultures();

        }
        private static void InitialiseCultures()
        {
            if (!string.IsNullOrEmpty(Settings.Default.Culture))
            {
                LocalizeDictionary.Instance.Culture
                    = Thread.CurrentThread.CurrentCulture
                    = new CultureInfo(Settings.Default.Culture);
            }

            if (!string.IsNullOrEmpty(Settings.Default.UICulture))
            {
                LocalizeDictionary.Instance.Culture
                    = Thread.CurrentThread.CurrentUICulture
                    = new CultureInfo(Settings.Default.UICulture);
            }

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeTheme();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Save the config file
            Settings.Default.Culture = LocalizeDictionary.Instance.Culture.Name;
            Settings.Default.Save();
            base.OnExit(e);
        }

        public static void InitializeTheme()
        {
            // Set theme property from default config value
            AppearanceManager.Current.ThemeSource = Settings.Default.Theme == "light" ? LightThemeSource : DarkThemeSource;

            // Set fontSize property from default config value
            AppearanceManager.Current.FontSize = Settings.Default.FontSize == "normal" ? FontSize.Small : FontSize.Large;

            // Set the accent color
            AppearanceManager.Current.AccentColor = ColorUtil.DrawingToMedia(Settings.Default.AccentColor);
        }


        public static readonly Uri LightThemeSource = new Uri("/Assets/Light.xaml", UriKind.Relative);
        public static readonly Uri DarkThemeSource = new Uri("/Assets/Dark.xaml", UriKind.Relative);

    }




}
