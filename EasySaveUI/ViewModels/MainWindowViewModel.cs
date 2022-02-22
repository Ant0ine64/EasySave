using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ICommand OnClickDelete { get; private set; }
        public ICommand OnClickStart { get; private set; }
        public ICommand OnClickSelectAll { get; private set; }
        private bool selectedAll = false;
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
            
            OnClickDelete = ReactiveCommand.Create(() =>
            {
                var jobs = Jobs.ToList();
                foreach (var job in jobs.Where(job => job.IsChecked))
                {
                    // remove visually
                    Jobs.Remove(job);
                    // remove in jobfile
                    Job.Delete(job);
                }
            });
            
            OnClickStart = ReactiveCommand.Create(() =>
            {
                foreach (var job in Jobs)
                {
                    job.IsChecked = selectedAll;
                }
            });
            
            OnClickSelectAll = ReactiveCommand.Create(() =>
            {
                selectedAll = !selectedAll;
                foreach (var job in Jobs)
                {
                    job.IsChecked = selectedAll;
                }
            });
        }
    }
}
