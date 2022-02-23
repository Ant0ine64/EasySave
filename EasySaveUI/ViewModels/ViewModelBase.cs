using EasySaveUI.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace EasySaveUI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// update the content and datacontext of MainWindow
        /// </summary>
        /// <param name="content">exemple:logsWindow.Content</param>
        /// <param name="dataContext">exmplelogsWindow.DataContext</param>
        public void updateContent(object content, object dataContext)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {               
                
                if (dataContext==null)
                {
                    dataContext = new MainWindowViewModel();
                }
                desktop.MainWindow.DataContext = dataContext;
                desktop.MainWindow.Content = content;
                
            }
        }
    }
}
