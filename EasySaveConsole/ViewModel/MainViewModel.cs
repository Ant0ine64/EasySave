using EasySaveConsole.Model;
using System.Globalization;
using System.Threading;
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
            // retourne un tableau contenant le nom des travaux
            return null;
        }

        public void deleteSavingJob(string name)
        {
            // supprime le travail de sauvegarde à partir du nom
        }

        public void executeSavingJob()
        {
            //execute tous les travail de sauvegarde
        }

        public void executeSavingJob(string name)
        {
            //execute le travail de sauvegarde à partir du nom 
        }
    }
}