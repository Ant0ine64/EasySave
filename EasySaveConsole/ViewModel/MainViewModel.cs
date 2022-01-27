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
            job = Job.GetJobByName(jobName);
            job.Status = "ACTIVE";
            Job.Update(job);
            
            if(job.Type == "P")
            {
                try {
                    DirectoryInfo infosDestDir = new DirectoryInfo(job.DestinationPath);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(job.SourcePath);
                    save.copyFilesPartialSave(infosSourceDir, infosDestDir);
                }
                catch
                {
                    Console.WriteLine(Properties.Resources.error_directory_path);
                }
            }
            else
            {
                try
                {
                    DirectoryInfo infosDestDir = new DirectoryInfo(job.DestinationPath);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(job.SourcePath);
                    save.copyFilesEntireSave(infosSourceDir, infosDestDir);
                }
                catch
                {
                    Console.WriteLine(Properties.Resources.error_directory_path);
                }
            }
            // write log file
            job.Status = "END";
            Job.Update(job);
        }

        public void CreateSavingJob(string name, string source, string destination, string type, string status="TODO")
        {
            job.Name = name;
            job.SourcePath = source;
            job.DestinationPath = destination;
            job.Type = type;
            job.Status = status;
            Job.Add(job);
        }
    }
}