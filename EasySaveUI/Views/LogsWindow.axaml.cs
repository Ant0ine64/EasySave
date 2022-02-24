using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveUI.ViewModels;

namespace EasySaveUI.Views
{
    public partial class LogsWindow : Window
    {
        public static LogsWindow Instance { get; private set; }
        public LogsWindow()
        {
            Instance = this;
            InitializeComponent();
            DataContext = new LogsWindowViewModel();
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
