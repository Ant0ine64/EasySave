using EasySaveConsole.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace EasySaveConsole.ViewModel
{

    /// <summary>
    /// Main and only view model, makes the link between View (prompt) and models
    /// </summary>
    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();

        /// <summary>
        /// Start a backup job using its name
        /// </summary>
        /// <param name="jobName">The name of the job you want to start</param>
        public void StartSavingJob(string jobName)
        {
            job = Job.GetJobByName(jobName);
            job.Status = "ACTIVE";
            Job.Update(job);

            LogFile.CreateFile();
            
            // Chose the type: d for differential, c for complete
            if(job.Type == "d")
            {
                try {
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

        /// <summary>
        /// Create a saving/backuping Job
        /// </summary>
        /// <param name="name">Name describing the job</param>
        /// <param name="source">Source path to save</param>
        /// <param name="destination">Destination of where to put saved files</param>
        /// <param name="type">Type of save: d for differential, c for complete</param>
        /// <param name="status">optional</param>
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

        /// <summary>
        /// Set language
        /// </summary>
        /// <param name="lang">language: fr or en</param>
        public void Translate(string lang)
        {
            CultureInfo ui_culture = new CultureInfo(lang);
            CultureInfo culture = new CultureInfo(lang);

            Thread.CurrentThread.CurrentUICulture = ui_culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        /// <summary>
        /// Obtains all the jobs
        /// </summary>
        /// <returns>job names</returns>
        public string[] FetchSavingJob()
        {
            string[] arrayJobsName;
            List<string> listJobsName = Job.GetAllJobNames();
            arrayJobsName = listJobsName.ToArray();
            return arrayJobsName;
        }

        /// <summary>
        /// Delete a job
        /// </summary>
        /// <param name="name">job name</param>
        public void DeleteSavingJob(string name)
        {
            Job.Delete(Job.GetJobByName(name));
        }

        /// <summary>
        /// Start all saving/backuping jobs
        /// </summary>
        public void StartAllSavingJobs()
        {
            Job.GetAllJobNames().ForEach(StartSavingJob);
        }

    }
}