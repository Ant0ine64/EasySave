using System;
using System.IO;
using System.Text.Json;

namespace EasySaveConsole.Model
{
    public class Log
    {
        public string Name { get; }
        public string FileSource { get; }
        public string FileTarget { get; }
        public string DestPath { get; }
        public long FileSize { get; set; }
        public long FileTransfertTime { get; }
        public DateTime Time { get; set; }

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