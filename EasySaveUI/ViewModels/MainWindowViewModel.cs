using Avalonia.Controls;
using EasySaveConsole.Model;
using EasySaveUI.Views;
using ReactiveUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace EasySaveUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Job job = new Job();
        public ICommand OnClickCreated { get; private set; }

       // public ICommand MyCombobox1_SelectionChanged { get; set; }

        public MainWindowViewModel()
        {
            
            List<Job> jobs = new List<Job>();
            List<string> jobsName = Job.GetAllJobNames();
            foreach (string job in jobsName)
            {
               Job monjob = Job.GetJobByName(job);
                jobs.Add(new Job() { Name = monjob.Name, SourcePath = monjob.SourcePath, Status = monjob.Status });
              //  Debug.WriteLine(monjob.Name);
            }


            OnClickCreated = ReactiveCommand.Create(() =>
            {
                CreatePage createPage = new CreatePage();
                createPage.Show();

            });
           
        }
        

    }
}
