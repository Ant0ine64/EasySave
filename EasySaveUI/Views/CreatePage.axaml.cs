using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;

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
