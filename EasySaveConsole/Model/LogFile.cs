using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EasySaveConsole.Properties;

namespace EasySaveConsole.Model
{

    public class LogFile
    {
        public static string directoryPath;
        public static string filePath;
        private static List<Log> logs = new List<Log>();

        public static void CreateFile()
        {
            directoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");
            string fileName = DateTime.Today.ToString("dd-MM-yyyy_")+ "log.json";
            filePath = Path.Join(directoryPath, fileName);
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
                Console.WriteLine(Resources.perm_error);
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