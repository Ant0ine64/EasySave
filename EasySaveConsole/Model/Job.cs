using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using EasySaveConsole.Properties;

namespace EasySaveConsole.Model
{

    /// <summary>
    /// A job represent a backup work
    /// </summary>
    public class Job : ModelBase
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        private string status;
        public string Status { get => status ; set => SetField(ref status, value,  nameof(Status)); }
        public int TotalFilesToCopy { get; set; }
        public int FilesLeftToDo { get; set; } = 0;
        public long TotalFilesSize { get; set; }
        private int progression = 0;
        public int Progression { get => progression ; set => SetField(ref progression, value,  nameof(Progression)); }
        public string Type { get; set; } = "c"; //c for complete, d for diferential
        private bool isChecked = false;
        public bool IsChecked { get => isChecked ; set => SetField(ref isChecked, value,  nameof(IsChecked)); }
        /// true if you want tu use cryptosoft
        public bool Cipher { get; set; } = false;

        public static string jsonStateDirectory;
        public static string jsonStateFilepath;

        private static string json;
        /// <summary>
        /// Contains all the jobs
        /// </summary>
        public static List<Job> Jobs = new List<Job>();

        /// <summary>
        /// Create job.json file if not exists
        /// </summary>
        public Job()
        {
            jsonStateDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");
            string fileName = "jobs.json";
            jsonStateFilepath = Path.Join(jsonStateDirectory, fileName);
            try
            {
                // create directory if not exists
                if (!Directory.Exists(jsonStateDirectory))
                    Directory.CreateDirectory(jsonStateDirectory);
                // create empty json file to start with
                if (!File.Exists(jsonStateFilepath))
                    File.WriteAllText(jsonStateFilepath, "[]");
            }
            catch
            {
                Console.WriteLine(Resources.perm_error);
            }
        }

        /// <summary>
        /// Add job to json state file
        /// </summary>
        /// <param name="job">instance to add into json</param>
        public static void Add(Job job)
        {
            GetFromJson();

            // complete job informations
            job.Complete();
            // add the new job 
            Jobs.Add(job);
            
            WriteToJson();
        }

        /// <summary>
        /// Write jobs static variable to json
        /// </summary>
        private static void WriteToJson()
        {
            // write jobs to json file
            json = JsonSerializer.Serialize(Jobs, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(jsonStateFilepath, json);
        }

        /// <summary>
        /// Fill jobs static variable with jobs from json
        /// </summary>
        public static void GetFromJson()
        {
            // get existing jobs from json
            string jsonJobs = File.ReadAllText(jsonStateFilepath);
            Jobs = JsonSerializer.Deserialize<List<Job>>(jsonJobs);
        }

        /// <summary>
        /// Complete your job attributes (fill TotalFilesToCopy and TotalFilesSize)
        /// </summary>
        private void Complete()
        {
            var files = Directory.GetFiles(this.SourcePath, "*", SearchOption.AllDirectories);
            TotalFilesToCopy = files.Count();
            TotalFilesSize = files.Sum(t => (new FileInfo(t).Length));

        }

        /// <summary>
        /// Update your job in the json
        /// </summary>
        /// <param name="job">Job you want to update</param>
        public static void Update(Job job)
        {
            GetFromJson();

            // find corresponding job (by name) and rewrite it
            int i = Jobs.FindIndex(j => j.Name == job.Name);
            Jobs[i] = job;
            
            WriteToJson();
        }

        /// <summary>
        /// Delete your job in the json
        /// </summary>
        /// <param name="job">Job you want to delete</param>
        public static void Delete(Job job)
        {
            GetFromJson();

            // find corresponding job (by name) and delete it
            int i = Jobs.FindIndex(j => j.Name == job.Name);
            Jobs.RemoveAt(i);
            
            WriteToJson();
        }

        /// <summary>
        /// Usage example : Job.GetAllJobNames().ForEach(Console.WriteLine);
        /// </summary>
        /// <returns>Names of all the jobs</returns>
        public static List<string> GetAllJobNames()
        {
            List<string> jobsName = new List<string>();
            
            GetFromJson();

            foreach (Job job in Jobs)
            {
                jobsName.Add(job.Name);
            }
            
            return jobsName;
        }

        /// <summary>
        /// Update FilesLeftTdo and Progression of your instance and write it to json
        /// </summary>
        public void UpdateProgression()
        {
            var sourceFiles = Directory.GetFiles(this.SourcePath, "*", SearchOption.AllDirectories).Count();
            var destFiles = Directory.GetFiles(this.DestinationPath, "*", SearchOption.AllDirectories).Count();
            FilesLeftToDo = sourceFiles - destFiles;
            Progression = (int)(((float)(sourceFiles - FilesLeftToDo) / (float)sourceFiles) * 100);
            Update(this);
        }

        /// <summary>
        /// Find a job by its name
        /// </summary>
        /// <param name="jobName">Name of the job</param>
        /// <returns>corresponding job</returns>
        public static Job? GetJobByName(string jobName)
        {
            return Jobs.Find(j => j.Name == jobName);
        }
    }
}