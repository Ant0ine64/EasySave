using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasySaveUI.Views;
using ReactiveUI;
using System.Windows.Input;
using EasySaveConsole.Model;
using EasySaveConsole.ViewModel;

namespace EasySaveUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnClickCreated { get; private set; }
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
        }
    }
}
