using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;
using System.Collections.Generic;
using Avalonia.VisualTree;

namespace EasySaveUI.Views
{

    public partial class Settings : Window
    {
        private SettingsWindowViewModel vm = new SettingsWindowViewModel();
        public Settings()
        {
            InitializeComponent();
            DataContext = vm;
            
            vm.SuccessChangedEvent += SuccessChangedEvent;
            vm.AddEvent += ClearAddBlockingApp;
                #if DEBUG
            this.AttachDevTools();
#endif
            this.Find<TextBox>("CryptoSoftPath").Text = vm.settings.CryptoSoftPath;
            SuccessChangedEvent(vm.GetLogFormat().ToUpper());
        }

        private void SuccessChangedEvent(string type)
        {
            switch(type)
            {
                case "XML":
                    this.Find<Button>("XML").IsEnabled = false;
                    this.Find<Button>("JSON").IsEnabled = true;
                    break;
                case "JSON":
                    this.Find<Button>("XML").IsEnabled = true;
                    this.Find<Button>("JSON").IsEnabled = false;
                    break;
            }
        }

        private void ClearAddBlockingApp()
        {
            this.Find<TextBox>("NewBlockingApp").Text = "";
        }

        public async void GetPathExe()
        {
            var dialogExeSource = new OpenFileDialog();
            dialogExeSource.Filters = new List<FileDialogFilter>() { new FileDialogFilter() { Name = "Exe", Extensions = { "exe" } } };
            var result = await dialogExeSource.ShowAsync(this);
            if (result == null || result.Length <= 0)
                return;
            this.Find<TextBox>("CryptoSoftPath").Text = result[0];
            vm.settings.CryptoSoftPath = result[0];
            vm.settings.Write();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DeleteApp_OnClick(object? sender, RoutedEventArgs e)
        {
            var vis = sender as IVisual;
            if (vis == null)
                return;
            while (true)
            {
                vis = vis.GetVisualParent();
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.IsVisible = false;
                    vm.DeleteBlockingApp((string?) row.DataContext);
                }
                if (vis is Window)
                    break;
            }
        }
    }
}
