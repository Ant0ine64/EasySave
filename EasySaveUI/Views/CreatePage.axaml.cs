using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;
using System;
using System.Diagnostics;

namespace EasySaveUI.Views
{
   
    public partial class CreatePage : Window
    {
        public static CreatePage Instance { get; private set; }

        public static bool resultError { get;  set; }


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

      
        public void errorMessage(object? sender, RoutedEventArgs e)
        {
            CreatePageViewModel vm = new CreatePageViewModel();
            vm.OnClickCreate.Execute(null);
            if (resultError)
            {
                TextBlock error = this.Find<TextBlock>("errorLabel");
                error.IsVisible = true;
            }
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
        }
    }
}
