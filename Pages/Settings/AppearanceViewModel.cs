﻿using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace SimplifiedPaint.Pages.Settings
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class AppearanceViewModel
        : NotifyPropertyChanged
    {
        private const string FontSmall = "normal";
        private const string FontLarge = "large";


        // 20 accent colors from Windows Phone 8
        private Color[] accentColors = new Color[]{
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
        };

        private Color selectedAccentColor;
        private LinkCollection themes = new LinkCollection();
        private Link selectedTheme;
        private string selectedFontSize;

        private CultureInfo[] languages = {
                new CultureInfo("en"), new CultureInfo("pl"),new CultureInfo("ru")};

        private CultureInfo selectedLanguage;

        public AppearanceViewModel()
        {

            // add the default themes
            this.themes.Add(new Link { DisplayName = "light", Source = App.LightThemeSource });
            this.themes.Add(new Link { DisplayName = "dark", Source = App.DarkThemeSource });
            this.SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FontLarge : FontSmall;

            // select proper language
            SelectedLanguage = languages.FirstOrDefault(l => l.Name.ToString() == Properties.Settings.Default.Culture);


            // Accent color has to be set to value choosen by user and written in config
            SelectedTheme = themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));
            AppearanceManager.Current.AccentColor = ColorUtil.DrawingToMedia(Properties.Settings.Default.AccentColor);
            SelectedAccentColor = AppearanceManager.Current.AccentColor;

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }



        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource")
            {
                // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
                this.SelectedTheme = this.themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));
            }

            if (e.PropertyName == "AccentColor")
            {
                // and make sure accent color is up-to-date
                this.SelectedAccentColor = AppearanceManager.Current.AccentColor;
            }

            // Save it to the config file
            Properties.Settings.Default.AccentColor = ColorUtil.MediaToDrawing(SelectedAccentColor);
            Properties.Settings.Default.Theme = SelectedTheme.DisplayName;
            Properties.Settings.Default.FontSize = SelectedFontSize;
        }

        public LinkCollection Themes
        {
            get { return this.themes; }
        }

        public string[] FontSizes
        {
            get { return new string[] { FontSmall, FontLarge }; }
        }

        public Color[] AccentColors
        {
            get { return this.accentColors; }
        }

        public Link SelectedTheme
        {
            get { return this.selectedTheme; }
            set
            {
                if (this.selectedTheme != value)
                {
                    this.selectedTheme = value;
                    OnPropertyChanged("SelectedTheme");

                    // and update the actual theme
                    AppearanceManager.Current.ThemeSource = value.Source;
                    AppearanceManager.Current.AccentColor = selectedAccentColor;
                }
            }
        }

        public string SelectedFontSize
        {
            get { return this.selectedFontSize; }
            set
            {
                if (this.selectedFontSize != value)
                {
                    this.selectedFontSize = value;
                    OnPropertyChanged("SelectedFontSize");

                    AppearanceManager.Current.FontSize = value == FontLarge ? FontSize.Large : FontSize.Small;
                }
            }
        }

        public Color SelectedAccentColor
        {
            get { return this.selectedAccentColor; }
            set
            {
                if (this.selectedAccentColor != value)
                {
                    this.selectedAccentColor = value;
                    OnPropertyChanged("SelectedAccentColor");
                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }


        public CultureInfo SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }

            set
            {

                if (selectedLanguage != value)
                {
                    selectedLanguage = value;
                    OnPropertyChanged("SelectedLanguage");
                    LocalizeDictionary.Instance.Culture = value;
                }

            }
        }

        public CultureInfo[] Languages
        {
            get
            {
                return languages;
            }

            set
            {
                languages = value;
            }
        }
    }
}
