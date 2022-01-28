using System;
using System.IO;
using System.Text.Json;

namespace EasySaveConsole.Model
{
    /// <summary>
    /// Represent a log entry in the logfile
    /// </summary>
    public class Log
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string DestPath { get; set; }
        public long FileSize { get; set; }
        public long FileTransfertTime { get; set; }
        public DateTime Time { get; set; }

        /// <summary>
        /// Constructor to create log entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileSource"></param>
        /// <param name="fileTarget"></param>
        /// <param name="destPath"></param>
        /// <param name="fileTransfertTime"></param>
        public Log(string name, string fileSource, string fileTarget, string destPath, long fileTransfertTime)
        {
            Name = name;
            FileSource = fileSource;
            FileTarget = fileTarget;
            DestPath = destPath;
            FileTransfertTime = fileTransfertTime;
            // complete the object
            FileSize = new FileInfo(FileSource).Length;
            Time = DateTime.Now;
        }

    
        /// <summary>
        /// Constructor for json deserializer 
        /// </summary>
        public Log()
        {

        }

        public string GetJson()
        {
            // return json
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true});
        }
    }
}