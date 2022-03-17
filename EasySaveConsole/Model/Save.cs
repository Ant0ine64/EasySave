using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace EasySaveConsole.Model
{

    public class Save
    {
        private Stopwatch watch = new Stopwatch();
        public bool Cipher = false;

        /// <summary>
        /// Copy files for a differential backup 
        /// </summary>
        /// <param name="infosSourceDir"></param>
        /// <param name="infosDestDir"></param>
        /// <param name="job">Saving job to execute</param>
        public void copyFilesPartialSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)
        {
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();
            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
          
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                // check the job state and quit the function if it is stopped
                if (CheckState(job)) return;

                // if the file exist in destination
                if (infosDestinationFiles.Any(x => x.Name == infosSourceFile.Name)) 
                {
                    // if the file which exist in destination has been edited in source 
                    if (infosDestinationFiles.Any(x => x.Name == infosSourceFile.Name && infosSourceFile.LastWriteTime != x.LastWriteTime)) 
                    {
                        watch.Start();
                        // replace existing file
                        ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                        watch.Stop();
                        Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                        LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
                    }
                }
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
            job.state = 0;
        }

        /// <summary>
        /// Copy file to create a complete backup
        /// </summary>
        /// <param name="infosSourceDir"></param>
        /// <param name="infosDestDir"></param>
        /// <param name="job">Saving job to execute</param>
        public void copyFilesEntireSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)
        {
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();

            // cleaning destination folder
            infosDestDir.Delete(true);
            infosDestDir.Create();

            //copy all files from source to destination
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                // check the job state and quit the function if it is stopped
                if (CheckState(job)) return;

                watch.Start();
                ExecuteSave(infosSourceDir, infosDestDir, infosSourceFile);
                watch.Stop();
                Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                job.UpdateProgression();
                LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
            }
            job.state = 0;
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

        private bool CheckState(Job job)
        {
            bool stop = false;
            // Thread.Sleep(2000); <-- pour tester la pause ou l'arret
            // if the job is stopped
            if (job.state == 0)
            {
                job.Status = "STOP";
                stop = true;
            }
            // if the job is on pause
            else if (job.state == 2)
            {
                job.Status = "PAUSE";
                while (job.state == 2) ;
                job.Status = "ACTIVE";
            }
            return stop;
        }
    }
}