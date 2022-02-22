using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public ICommand OnClickRefresh { get; private set; }
        private bool selectedAll = false;
        public MainViewModel mvm = new MainViewModel();
        public ObservableCollection<Job> Jobs { get; set; }

        public MainWindowViewModel()
        {
            Jobs = new ObservableCollection<Job>(mvm.fetchSavingJob());
            foreach (var job in Jobs)
            {
                job.IsChecked = false;
            }
            
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
            
            OnClickStart = ReactiveCommand.Create(async () =>
            {
                await Task.WhenAll(Jobs.Where(job => job.IsChecked).Select(async j => await mvm.StartSavingJob(j)));
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
