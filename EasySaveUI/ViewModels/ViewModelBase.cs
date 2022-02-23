using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace EasySaveUI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public void updateContent(object content, object dataContext)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Content = content;
                desktop.MainWindow.DataContext = dataContext;
            }
        }
    }
}
