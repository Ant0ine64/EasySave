using EasySaveConsole.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
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

            LogFile.CreateFile();
            
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

        public string[] fetchSavingJob()
        {
            string[] arrayJobsName;
            // List<string> listJobsName = new List<string>(new string[] { "Saving job 0", "Saving job 1", "Saving job 2" }); // uniquement pour tester la fonction, � supprimer avant le merge
            List<string> listJobsName = Job.GetAllJobNames();
            arrayJobsName = listJobsName.ToArray();
            return arrayJobsName;
        }

        public void deleteSavingJob(string name)
        {
            Console.WriteLine(name + " a été supprimé avec succés");
        }

        public void executeSavingJob()
        {
            Console.WriteLine("tous les travaux de sauvegarde sont en cours d'executions");
            //execute tous les travaux de sauvegarde
        }

        public void executeSavingJob(string name)
        {
            Console.WriteLine("Le travail " + name +  " est en cours d'execution");
            //execute le travail de sauvegarde � partir du nom 
        }
    }
}