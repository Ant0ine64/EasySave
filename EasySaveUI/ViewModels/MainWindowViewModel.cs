using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasySaveUI.Views;
using ReactiveUI;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using EasySaveConsole.Model;
using EasySaveConsole.ViewModel;
using Settings = EasySaveUI.Views.Settings;
using Avalonia.Controls.ApplicationLifetimes;

namespace EasySaveUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnClickCreated { get; private set; }
        public ICommand OnClickDelete { get; private set; }
        public ICommand OnClickStart { get; private set; }
        public ICommand OnClickPause { get; private set; }
        public ICommand OnClickStop { get; private set; }
        public ICommand OnClickSelectAll { get; private set; }
        private bool selectedAll = false;
        public ICommand OnClickLogs { get; private set; }
        public MainViewModel mvm = new MainViewModel();
        public ObservableCollection<Job> Jobs { get; set; }
        public ICommand OnClickSettings { get; private set; }

        public string CryptosoftPassword { private get; set; } = "";

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
                updateContent(createPage.Content, createPage.DataContext);
                createPage.Close();
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
                var checkedJobs = Jobs.Where(job => job.IsChecked);
                foreach (Job checkedJob in checkedJobs)
                {
                    if (checkedJob.state == 0)
                    {
                        checkedJob.state = 1;
                        Task.Run(() => mvm.StartSavingJob(checkedJob));
                    }
                    else
                    {
                        checkedJob.state = 1;
                    }
                }
            });

            OnClickPause = ReactiveCommand.Create(() =>
            {
                var checkedJobs = Jobs.Where(job => job.IsChecked);
                foreach (Job checkedJob in checkedJobs)
                {
                    if (checkedJob.state == 1) checkedJob.state = 2;
                }
            });

            OnClickStop = ReactiveCommand.Create(() =>
            {
                var checkedJobs = Jobs.Where(job => job.IsChecked);
                foreach (Job checkedJob in checkedJobs)
                {
                    if (checkedJob.state !=0) checkedJob.state = 0;
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

            OnClickSettings = ReactiveCommand.Create(() =>
            {
                Settings settingsPage = new Settings();
                updateContent(settingsPage.Content, settingsPage.DataContext);
                settingsPage.Close();
            });

            OnClickLogs = ReactiveCommand.Create(() =>
            {
                LogsWindow logsWindow = new LogsWindow();
                updateContent(logsWindow.Content, logsWindow.DataContext);
                logsWindow.Close();

            });
        }
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add "using Windows.UI;" for Color and Colors.
            string colorName = e.AddedItems[0].ToString();
            Debug.WriteLine(colorName);
        }
    }
}
