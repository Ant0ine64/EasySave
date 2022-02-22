using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;
using System;

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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
