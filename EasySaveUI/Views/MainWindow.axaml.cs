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
            Button pause = this.Find<Button>("pause");
            Button stop = this.Find<Button>("stop");
            Button settings = this.Find<Button>("settings");
            Button add_all = this.Find<Button>("add_all");
            DataGrid list_jobs = this.Find<DataGrid>("ListJobs");




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
                pause.Content = "Pause";
                stop.Content = "Stop";
                settings.Content = "Settings";
                add_all.Content = "Select All";
                list_jobs.Columns[0].Header = "Select";
                list_jobs.Columns[1].Header = "Name";             
                list_jobs.Columns[5].Header = "Progress";


            }
            if (selected == 1 && Thread.CurrentThread.CurrentUICulture != CultureInfo.GetCultureInfo("fr-FR"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
                Debug.WriteLine("fr");
                var lang = Thread.CurrentThread.CurrentUICulture;
                Debug.WriteLine(lang);
                create.Content = "Cr??er";
                delete.Content = "Supprimer";
                start.Content = "D??marrer";
                pause.Content = "Pause";
                stop.Content = "Stop";
                settings.Content = "Param??tres";
                add_all.Content = "Tout s??lectionner";
                list_jobs.Columns[0].Header = "Selectionner";
                list_jobs.Columns[1].Header = "Nom";             
                list_jobs.Columns[5].Header = "Progression";
            }

        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
