using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;
using System.Collections.Generic;

namespace EasySaveUI.Views
{

    public partial class Settings : Window
    {

        public Settings()
        {
            var vm = new SettingsPageViewModel();
            InitializeComponent();
            DataContext = vm;
            vm.SuccessChangedEvent += SuccessChangedEvent;
                #if DEBUG
            this.AttachDevTools();
#endif
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

        public async void GetPathExe()
        {
            var dialogExeSource = new OpenFileDialog();
            dialogExeSource.Filters = new List<FileDialogFilter>() { new FileDialogFilter() { Name = "Exe", Extensions = { "exe" } } };
            var result = await dialogExeSource.ShowAsync(this);
            if (result == null || result.Length <= 0)
                return;
            this.Find<TextBox>("CryptoSoftPath").Text = result[0];
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
