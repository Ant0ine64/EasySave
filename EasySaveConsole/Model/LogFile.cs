using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EasySaveConsole.Properties;
using System.Xml.Serialization;

namespace EasySaveConsole.Model
{

    /// <summary>
    /// Represents the json logfile
    /// </summary>
    public class LogFile
    {
        public static string directoryPath;
        public static string filePath;
        private static List<Log> logs = new List<Log>();
        public static string selectLogFormat = "xml";

        /// <summary>
        /// Create the date_log.json file in appdata (or .config on linux)
        /// </summary>
        public static void CreateFile()
        {
            if (selectLogFormat == "json")
            { 
                directoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");
                string fileNameJson = DateTime.Today.ToString("dd-MM-yyyy_")+ "log.json";
                filePath = Path.Join(directoryPath, fileNameJson);
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

            else if (selectLogFormat == "xml")
            { 
                directoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");
                string fileNameXML = DateTime.Today.ToString("dd-MM-yyyy_") + "log.xml";
                filePath = Path.Join(directoryPath, fileNameXML);
                try
                {
                    // create directory temp if not exists
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    // create empty log.txt file to start with
                    if (!File.Exists(filePath))
                        File.WriteAllText(filePath, "<?xml version=\"1.0\"?>"
                            + Environment.NewLine + "</ Logs > ");
                }
                catch
                {
                    Console.WriteLine(Resources.perm_error);
                }
            }
        }

        /// <summary>
        /// Write new entry into log file
        /// </summary>
        /// <param name="job"></param>
        /// <param name="fileSource"></param>
        /// <param name="fileTarget"></param>
        /// <param name="fileTrafereTime">time to copy the file</param>
        public static void WriteToLog(Job job, string fileSource, string fileTarget, long fileTrafereTime)
        {
            if (selectLogFormat == "json")
            {
                string jsonLogs = File.ReadAllText(filePath);
                logs = JsonSerializer.Deserialize<List<Log>>(jsonLogs);
                Log log = new Log(job.Name, fileSource, fileTarget, job.DestinationPath, fileTrafereTime);
                logs.Add(log);
                string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }

            else if (selectLogFormat == "xml")
            {
                using (var xmlLogs = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Log>));
                    //logs = (List<Log>)serializer.Deserialize(xmlLogs);
                    Log log = new Log(job.Name, fileSource, fileTarget, job.DestinationPath, fileTrafereTime);
                    logs.Add(log);
                    serializer.Serialize(xmlLogs, logs);
                }
            }
        }
    }
}