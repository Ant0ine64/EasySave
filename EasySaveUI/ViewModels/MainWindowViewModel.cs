using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasySaveUI.Views;
using ReactiveUI;
using System.Windows.Input;
using EasySaveConsole.Model;
using EasySaveConsole.ViewModel;
using Avalonia.Controls.ApplicationLifetimes;

namespace EasySaveUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnClickCreated { get; private set; }

        public ICommand LogsButton { get; private set; }

        public MainViewModel mvm = new MainViewModel();
        public ObservableCollection<Job> Jobs { get; set; }

        public MainWindowViewModel()
        {
            
            Jobs = new ObservableCollection<Job>(mvm.fetchSavingJob());
            OnClickCreated = ReactiveCommand.Create(() =>
            {
                CreatePage createPage = new CreatePage();
                createPage.Show();
            });

            LogsButton = ReactiveCommand.Create(() =>
            {
                LogsWindow logsWindow = new LogsWindow();
                updateContent(logsWindow.Content, logsWindow.DataContext);

            });
        }
    }
}
