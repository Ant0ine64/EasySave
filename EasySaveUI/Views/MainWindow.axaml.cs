using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace EasySaveUI.Views
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
           
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TextBlock create = this.Find<TextBlock>("create");


            ComboBox combo = (ComboBox)sender;
                // Add "using Windows.UI;" for Color and Colors.
                var selected = combo.SelectedIndex;
                if (selected == 2 && Thread.CurrentThread.CurrentUICulture != CultureInfo.GetCultureInfo("en-US"))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                    Debug.WriteLine("en");
                    var lang = Thread.CurrentThread.CurrentUICulture;
                    Debug.WriteLine(lang);
                   
                   

                }
                if (selected == 1 && Thread.CurrentThread.CurrentUICulture != CultureInfo.GetCultureInfo("fr-FR"))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
                    Debug.WriteLine("fr");
                    var lang = Thread.CurrentThread.CurrentUICulture;
                    Debug.WriteLine(lang);
                    create.Text = "Créer";

            }
            
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
