using EasySaveConsole.Model;
using System;
using System.IO;

namespace EasySaveConsole.ViewModel
{

    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();

        public void StartSavingJob(string jobName)
        {
            job.Name = jobName;
            // beginning of the job execution
            job.Status = "ACTIVE";
            Job.Update(job);
            
            
            
            // write log file
            job.Status = "END";
            Job.Update(job);
        }

        public void CreateSavingJob(string name, string source, string destination, string status="TODO")
        {
            job.Name = name;
            job.SourcePath = source;
            job.DestinationPath = destination;
            job.Status = status;
            Job.Add(job);
        }
    }
}