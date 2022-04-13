using EasySaveConsole.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasySaveConsole.ViewModel
{

    /// <summary>
    /// Main and only view model, makes the link between View (prompt) and models
    /// </summary>
    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();
        private CryptoSoft cryptoSoft = CryptoSoft.GetInstance();
        private Settings settings = new Settings(true);

        public MainViewModel()
        {
            // init settings 
            settings.ReadSettings();
           
        }

        /// <summary>
        /// Start a backup job using its name
        /// </summary>
        /// <param name="jobName">The name of the job you want to start</param>
        public async void StartSavingJob(string jobName)
        {
            
            job = Job.GetJobByName(jobName);
            StartSavingJob(job);
        }

        public void StartSavingJob(Job job)
        {
            save.Cipher = job.Cipher;
            SetXorKey();

            job.Status = "ACTIVE";
            job.Progression = 0;
            Job.Update(job);
            
            if (!CheckBlockingApps())
            {
                Console.WriteLine("Blocking app detected");
                job.Status = "ABORTED";
                job.state = 0;
                Job.Update(job);
                return;
            }

            LogFile.CreateFile();
            // Chose the type: d for differential, c for complete
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
                catch(Exception e)
                {
                    Console.WriteLine(e + Environment.NewLine + Properties.Resources.error_directory_path);

                }
            }
            // write log file
            if (job.Status != "STOP") job.Status = "END";
            Job.Update(job);
        }

        /// <summary>
        /// Create a saving/backuping Job
        /// </summary>
        /// <param name="name">Name describing the job</param>
        /// <param name="source">Source path to save</param>
        /// <param name="destination">Destination of where to put saved files</param>
        /// <param name="type">Type of save: d for differential, c for complete</param>
        /// <param name="cipher"></param>
        /// <param name="status">optional</param>
        public void CreateSavingJob(string name, string source, string destination, string type,
            List<string>? cipher = null,
            string status = "TODO")
        {
            job.Name = name;
            job.SourcePath = source;
            job.DestinationPath = destination;
            job.Type = type;
            job.Status = status;
            job.Cipher = cipher;
            Job.Add(job);
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
        public string[] fetchSavingJobNames()
        {
            string[] arrayJobsName;
            List<string> listJobsName = Job.GetAllJobNames();
            arrayJobsName = listJobsName.ToArray();
            return arrayJobsName;
        }

        public IEnumerable<Job> fetchSavingJob()
        {
            Job.GetFromJson();
            return Job.Jobs;
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

        public void SetXorKey()
        {
            cryptoSoft.Key = settings.CryptoKey;
            if (settings.CryptoSoftPath != "")
                cryptoSoft.CryptoSoftPath = settings.CryptoSoftPath;
            else
            {
                Console.Error.WriteLine($"Please set your CryptoSoft executable in {Settings.SettingsFile}");
                throw new FileNotFoundException();
            }
        }

        public void SelectLogFormat(string LogFormat)
        {
            LogFile.selectLogFormat = LogFormat;
        }

        /// <summary>
        /// returns the path of daily log file
        /// </summary>
        /// <returns>log file path</returns>
        public string returnLogFilePath(string logFormat)
        {
            string FilePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave") + DateTime.Today.ToString("dd-MM-yyyy_") + "log." + logFormat;
            return FilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if save can be started</returns>
        private static bool CheckBlockingApps()
        {
            // get blocking app from settings
            var settings = new Settings();
            settings.ReadSettings();
            var blockingApps = settings.BlockingApp;
            
            if (blockingApps.Count == 0)
                return true;

            var processes = Process.GetProcesses().ToList();
            return processes.All(process => !blockingApps.Contains(process.ProcessName));
        }
    }
}