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

            Button create = this.Find<Button>("create");
            Button delete = this.Find<Button>("delete");
            Button start = this.Find<Button>("start");
            Button decrypt = this.Find<Button>("decrypt");
            Button settings = this.Find<Button>("settings");
            Button add_all = this.Find<Button>("add_all");




            ComboBox combo = (ComboBox)sender;
            // Add "using Windows.UI;" for Color and Colors.
            var selected = combo.SelectedIndex;
            if (selected == 2 && Thread.CurrentThread.CurrentUICulture != CultureInfo.GetCultureInfo("en-US"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                Debug.WriteLine("en");
                var lang = Thread.CurrentThread.CurrentUICulture;
                Debug.WriteLine(lang);
                create.Content = "Create";
                delete.Content = "Delete";
                start.Content = "Start";
                decrypt.Content = "Decrypt";
                settings.Content = "Settings";
                add_all.Content = "Select All";


            }
            if (selected == 1 && Thread.CurrentThread.CurrentUICulture != CultureInfo.GetCultureInfo("fr-FR"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
                Debug.WriteLine("fr");
                var lang = Thread.CurrentThread.CurrentUICulture;
                Debug.WriteLine(lang);
                create.Content = "Créer";
                delete.Content = "Supprimer";
                start.Content = "Démarrer";
                decrypt.Content = "Décrypter";
                settings.Content = "Paramètres";
                add_all.Content = "Tout sélectionner";
               

            }

        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
