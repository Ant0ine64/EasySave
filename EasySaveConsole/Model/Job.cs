using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using EasySaveConsole.Properties;

namespace EasySaveConsole.Model
{

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
        public int Progression { get; set; } = 0;
        public string Type { get; set; } = "c"; //c for complete, d for diferential
        private bool isChecked = false;
        public bool IsChecked { get => isChecked ; set => SetField(ref isChecked, value,  nameof(IsChecked)); }

        public static string jsonStateDirectory;
        public static string jsonStateFilepath;

        private static string json;
        public static List<Job> Jobs = new List<Job>();

        public Job()
        {
            jsonStateDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");
            Debug.WriteLine("path " + jsonStateDirectory);
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

        public static bool Add(Job job)
        {
            GetFromJson();

            // complete job informations
            job.Complete();
            // add the new job 
            Jobs.Add(job);
            // Only take last 5 elements : required on version 1
            if (Jobs.Count > 5)
                Jobs = Enumerable.Reverse(Jobs).Take(5).Reverse().ToList();
            
            WriteToJson();
            
            return true;
        }

        private static void WriteToJson()
        {
            // write jobs to json file
            json = JsonSerializer.Serialize(Jobs, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(jsonStateFilepath, json);
        }

        public static void GetFromJson()
        {
            // get existing jobs from json
            string jsonJobs = File.ReadAllText(jsonStateFilepath);
            Jobs = JsonSerializer.Deserialize<List<Job>>(jsonJobs);
        }

        private void Complete()
        {
            var files = Directory.GetFiles(this.SourcePath, "*", SearchOption.AllDirectories);
            TotalFilesToCopy = files.Count();
            TotalFilesSize = files.Sum(t => (new FileInfo(t).Length));

        }

        public static void Update(Job job)
        {
            GetFromJson();

            // find corresponding job (by name) and rewrite it
            int i = Jobs.FindIndex(j => j.Name == job.Name);
            Jobs[i] = job;
            
            WriteToJson();
        }

        public static void Delete(Job job)
        {
            GetFromJson();

            // find corresponding job (by name) and delete it
            int i = Jobs.FindIndex(j => j.Name == job.Name);
            Jobs.RemoveAt(i);
            
            WriteToJson();
        }

        /**
         * Usage example :
         *  Job.GetAllJobNames().ForEach(Console.WriteLine);
         */
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

        public void UpdateProgression()
        {
            var sourceFiles = Directory.GetFiles(this.SourcePath, "*", SearchOption.AllDirectories).Count();
            var destFiles = Directory.GetFiles(this.DestinationPath, "*", SearchOption.AllDirectories).Count();
            FilesLeftToDo = sourceFiles - destFiles;
            Progression = ((sourceFiles - FilesLeftToDo) / sourceFiles) * 100;
            Update(this);
        }

        public static Job? GetJobByName(string jobName)
        {
            return Jobs.Find(j => j.Name == jobName);
        }
    }
}