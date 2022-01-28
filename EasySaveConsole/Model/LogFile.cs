using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EasySaveConsole.Model
{

    public class LogFile
    {
        public const string directoryPath = "c:\\temp\\";
        public const string filePath = directoryPath + "Log.json";
        private static List<Log> logs = new List<Log>();

        public static void CreateFile()
        {
            try
            {
                // create directory temp if not exists
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // create empty log.txt file to start with
                if (!File.Exists(filePath))
                    File.WriteAllText(filePath, "[]");
            }
            catch
            {
                Console.WriteLine("Erreur de permission");
            }
        }

        public static void WriteToLog(Job job, string fileSource, string fileTarget, long fileTrafereTime)
        {
            string jsonLogs = File.ReadAllText(filePath);
            logs = JsonSerializer.Deserialize<List<Log>>(jsonLogs);
            Log log = new Log(job.Name, fileSource, fileTarget, job.DestinationPath, fileTrafereTime);
            //string Json = log.GetJson();
            logs.Add(log);
            string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}