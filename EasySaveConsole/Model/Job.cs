using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EasySaveConsole.Model
{

    public class Job
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string Status { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int FilesLeftToDo { get; set; } = 0;
        public long TotalFilesSize { get; set; }
        public int Progression { get; set; } = 0;

        public const string jsonStateDirectory = "/var/log/easysave/";
        public const string jsonStateFilepath = jsonStateDirectory + "jobs.json";

        private static string json;
        private static List<Job> jobs = new List<Job>();

        public Job()
        {
            // create directory if not exists
            if (!Directory.Exists(jsonStateDirectory))
                Directory.CreateDirectory(jsonStateDirectory);
            // create empty json file to start with
            if (!File.Exists(jsonStateFilepath))
                File.WriteAllText(jsonStateFilepath, "[]");
        }

        public static bool Add(Job job)
        {
            GetFromJson();

            // complete job informations
            job.Complete();
            // add the new job 
            jobs.Add(job);
            // Only take last 5 elements : required on version 1
            if (jobs.Count > 5)
                jobs = Enumerable.Reverse(jobs).Take(5).Reverse().ToList();
            
            WriteToJson();
            
            return true;
        }

        private static void WriteToJson()
        {
            // write jobs to json file
            json = JsonSerializer.Serialize(jobs, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(jsonStateFilepath, json);
        }

        private static void GetFromJson()
        {
            // get existing jobs from json
            string jsonJobs = File.ReadAllText(jsonStateFilepath);
            jobs = JsonSerializer.Deserialize<List<Job>>(jsonJobs);
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
            int i = jobs.FindIndex(j => j.Name == job.Name);
            jobs[i] = job;
            
            WriteToJson();
        }

        public static void Delete(Job job)
        {
            GetFromJson();

            // find corresponding job (by name) and delete it
            int i = jobs.FindIndex(j => j.Name == job.Name);
            jobs.RemoveAt(i);
            
            WriteToJson();
        }
    }
}