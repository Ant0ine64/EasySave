using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EasySaveConsole.Model
{

    public class Save
    {
        private Stopwatch watch = new Stopwatch();
        public bool cipher = true;

        /// <summary>
        /// Copy files for a differential backup 
        /// </summary>
        /// <param name="infosSourceDir"></param>
        /// <param name="infosDestDir"></param>
        /// <param name="job">Saving job to execute</param>
        public void copyFilesPartialSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)
        {
       
            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();
          
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                foreach (FileInfo infosDestinationFile in infosDestinationFiles)
                {
                    // if file have been modified
                    if (infosSourceFile.Name == infosDestinationFile.Name && infosSourceFile.LastWriteTime != infosDestinationFile.LastWriteTime)
                    { 
                        watch.Start();
                        // replace existing file
                        ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                        watch.Stop();
                        Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                        LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
                    }
                }
                // if file doesn't exist in destination
                if (infosDestinationFiles.Any(x => x.Name == infosSourceFile.Name)) { }
                else
                {
                    watch.Start();
                    ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                    watch.Stop();
                    Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                    LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
                }
                job.UpdateProgression();
            }
        }

        /// <summary>
        /// Copy file to create a complete backup
        /// </summary>
        /// <param name="infosSourceDir"></param>
        /// <param name="infosDestDir"></param>
        /// <param name="job">Saving job to execute</param>
        public void copyFilesEntireSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)
        {

            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();

            // cleaning destination folder
            foreach (FileInfo infosDestinationFile in infosDestinationFiles)
            {
                infosDestinationFile.Delete();              
            }

            //copy all files from source to destination
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                watch.Start();
                ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                watch.Stop();
                Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                job.UpdateProgression();
                LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
            }
        }

        private void ExecuteSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, FileInfo infosSourceFile)
        {
            string source = Path.Combine(infosSourceDir.FullName, infosSourceFile.Name);
            string dest = Path.Combine(infosDestDir.FullName, infosSourceFile.Name);
            
            if (!cipher)
                File.Copy(source, dest, true);
            else
                CryptoSoft.GetInstance().XorCypher(source, dest);
        }
    }
}