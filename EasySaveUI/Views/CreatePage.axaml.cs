using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;
using System;

namespace EasySaveUI.Views
{
   
    public partial class CreatePage : Window
    {
        public static CreatePage Instance { get; private set; }
        public CreatePage()
        {
            Instance = this;
            InitializeComponent();
            DataContext = new CreatePageViewModel();
            CreatePageViewModel vm = new CreatePageViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
        }
    }
}
