using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EasySaveConsole.Model
{

    public class Save
    {
        private Stopwatch watch = new Stopwatch();
        public bool Cipher = false;
        public EasySaveConsole.Model.Settings settings = new EasySaveConsole.Model.Settings(true);


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
            List<FileInfo> infosSourceFilesSortByLenght;
            List<FileInfo> infosSourceFilesSortByLenghtAndExtension = new List<FileInfo>();

            infosSourceFilesSortByLenght = infosSourceFiles.OrderByDescending(x => x.Length).ToList();
            settings.ReadSettings();
            var exentions = settings.PrioritaryExtension;

            for (int i = infosSourceFilesSortByLenght.Count - 1; i >= 0; i--)
            {
                if (exentions.Contains(infosSourceFilesSortByLenght[i].Extension))
                {
                    infosSourceFilesSortByLenghtAndExtension.Add(infosSourceFilesSortByLenght[i]);
                    infosSourceFilesSortByLenght.Remove(infosSourceFilesSortByLenght[i]);
                }
                else
                {
                    Debug.WriteLine("non-priority file");
                }
            }

            infosSourceFilesSortByLenghtAndExtension.AddRange(infosSourceFilesSortByLenght);

            foreach (FileInfo infosSourceFile in infosSourceFilesSortByLenghtAndExtension)
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
            List<FileInfo> infosSourceFilesSortByLenght;
            List<FileInfo> infosSourceFilesSortByLenghtAndExtension = new List<FileInfo>();

            infosSourceFilesSortByLenght = infosSourceFiles.OrderByDescending(x => x.Length).ToList();
            settings.ReadSettings();
            var exentions = settings.PrioritaryExtension;

            for (int i = infosSourceFilesSortByLenght.Count - 1; i >= 0; i--)
            {
                if (exentions.Contains(infosSourceFilesSortByLenght[i].Extension))
                {
                    infosSourceFilesSortByLenghtAndExtension.Add(infosSourceFilesSortByLenght[i]);
                    infosSourceFilesSortByLenght.Remove(infosSourceFilesSortByLenght[i]);
                }
                else
                {
                    Debug.WriteLine("non-priority file");
                }
            }

            infosSourceFilesSortByLenghtAndExtension.AddRange(infosSourceFilesSortByLenght);

            // cleaning destination folder
            foreach (FileInfo infosDestinationFile in infosDestinationFiles)
            {
                infosDestinationFile.Delete();              
            }

            //copy all files from source to destination
            foreach (FileInfo infosSourceFile in infosSourceFilesSortByLenghtAndExtension)
            {
                Debug.WriteLine(infosSourceFile.Name);
                watch.Start();
                ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                watch.Stop();
                Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                job.UpdateProgression();
                LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
             
            }
        }

        /// <summary>
        /// Choose whether to do a normal save of user xor cipher from cryptosoft
        /// </summary>
        /// <param name="infosSourceDir"></param>
        /// <param name="infosDestDir"></param>
        /// <param name="infosSourceFile"></param>
        private void ExecuteSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, FileInfo infosSourceFile)
        {
            string source = Path.Combine(infosSourceDir.FullName, infosSourceFile.Name);
            string dest = Path.Combine(infosDestDir.FullName, infosSourceFile.Name);
      
                if (!Cipher)
                    File.Copy(source, dest, true);
                else
                    CryptoSoft.GetInstance().XorCypher(source, dest);

            
        }
        static long GetFileLength(System.IO.FileInfo fi)
        {
            long retval;
            try
            {
                retval = fi.Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                // If a file is no longer present,  
                // just add zero bytes to the total.  
                retval = 0;
            }
            return retval;
        }
    }
}