using EasySaveUI.Views;
using ReactiveUI;
using System.Windows.Input;

namespace EasySaveUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnClickCreated { get; private set; }

        public MainWindowViewModel()
        {
            OnClickCreated = ReactiveCommand.Create(() =>
            {
                CreatePage createPage = new CreatePage();
                createPage.Show();
            });
        }
    }
}
