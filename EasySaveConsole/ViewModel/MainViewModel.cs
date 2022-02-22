using EasySaveConsole.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EasySaveConsole.ViewModel
{

    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();

        public void StartSavingJob(string jobName)
        {
            job = Job.GetJobByName(jobName);
            StartSavingJob(job);
        }

        public async Task StartSavingJob(Job job)
        {
            job.Status = "ACTIVE";
            Job.Update(job);
            var rand = new Random();
            ;
            await Task.Delay(rand.Next(2000, 5000));

            LogFile.CreateFile();

            if (job.Type == "d")
            {
                try
                {
                    DirectoryInfo infosDestDir = new DirectoryInfo(job.DestinationPath);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(job.SourcePath);
                    save.copyFilesPartialSave(infosSourceDir, infosDestDir, job);
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
                    save.copyFilesEntireSave(infosSourceDir, infosDestDir, job);
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
            //Save
            //Write state file 

        }

        public void translate(string lang)
        {
            CultureInfo ui_culture = new CultureInfo(lang);
            CultureInfo culture = new CultureInfo(lang);

            Thread.CurrentThread.CurrentUICulture = ui_culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public string[] fetchSavingJobNames()
        {
            string[] arrayJobsName;
            // List<string> listJobsName = new List<string>(new string[] { "Saving job 0", "Saving job 1", "Saving job 2" }); // uniquement pour tester la fonction, � supprimer avant le merge
            List<string> listJobsName = Job.GetAllJobNames();
            arrayJobsName = listJobsName.ToArray();
            return arrayJobsName;
        }

        public IEnumerable<Job> fetchSavingJob()
        {
            Job.GetFromJson();
            return Job.Jobs;
        }

        public void deleteSavingJob(string name)
        {
            Job.Delete(Job.GetJobByName(name));
        }

        public void StartAllSavingJobs()
        {
            Job.GetAllJobNames().ForEach(StartSavingJob);
        }

    }
}