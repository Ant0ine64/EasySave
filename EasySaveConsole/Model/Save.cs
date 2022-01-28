using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EasySaveConsole.Model
{

    public class Save
    {
        private Stopwatch watch = new Stopwatch();

        public void copyFilesPartialSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)// sauvegarde partielle
        {

       
            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();
          
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                foreach (FileInfo infosDestinationFile in infosDestinationFiles)
                {

                    // si le fichier a été modifié (nom est le meme mais pas l'heure de modif)
                    if (infosSourceFile.Name == infosDestinationFile.Name && infosSourceFile.LastWriteTime != infosDestinationFile.LastWriteTime)
                    { 
                        watch.Start();
                        // copie en écrasant la version existante  
                        File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                        watch.Stop();
                        Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                        LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);
                    }
                   

                }
                // si le fichier n'existe pas dans le destination on le copie
                if (infosDestinationFiles.Any(x => x.Name == infosSourceFile.Name)) { }
                else
                {
                    watch.Start();
                    File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                    watch.Stop();
                    Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                    LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);

                }
                job.UpdateProgression();
               
            }
           
        }

        public void copyFilesEntireSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir, Job job)// Sauvegarde entière
        {

            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();

            foreach (FileInfo infosDestinationFile in infosDestinationFiles)// clean du dossier de destination
            {
                infosDestinationFile.Delete();              

            }

            foreach (FileInfo infosSourceFile in infosSourceFiles)// copie de tous les fichiers du sorce vers le destination
            {
                watch.Start();
                File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                watch.Stop();
                Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                job.UpdateProgression();
                LogFile.WriteToLog(job, infosSourceFile.FullName, Path.Combine(infosDestDir.FullName, infosSourceFile.Name), watch.ElapsedMilliseconds);

            }
        }
    }
}