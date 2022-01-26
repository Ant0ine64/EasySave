using EasySaveConsole.Model;
using System;
using System.IO;

namespace EasySaveConsole.ViewModel
{

    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();

        public void StartSavingJob(string jobName)
        {
          

            // write log file
        }

        public void CreateSavingJob(string name, string source, string destination)
        {
            Console.WriteLine("Your save will be partial or entire (P/E)");
            if(Console.ReadLine() == "P")
            {
                try {
                    DirectoryInfo infosDestDir = new DirectoryInfo(destination);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(source);
                   
                    save.copyFilesPartialSave(infosSourceDir, infosDestDir);
                }
                catch
                {
                    Console.WriteLine("Le dossier source ou de destination n'existe pas");
                }
                
               
            }
            else
            {
                
                try
                {
                    DirectoryInfo infosDestDir = new DirectoryInfo(destination);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(source);
                   
                    save.copyFilesEntireSave(infosSourceDir, infosDestDir);
                }
                catch
                {
                    Console.WriteLine("Le dossier source ou de destination n'existe pas");
                }
            }
           
        }
    }
}