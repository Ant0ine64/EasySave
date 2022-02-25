using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DynamicData.Binding;
using EasySaveUI.ViewModels;

namespace EasySaveUI.Views
{
    public partial class CrptosoftDialog : Window
    {
        public CrptosoftDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // this.WhenValueChanged(dialog => dialog());
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    
    
}