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
            Console.WriteLine(Properties.Resources.type_save_question);
            if(Console.ReadLine() == "P")
            {
                try {
                    DirectoryInfo infosDestDir = new DirectoryInfo(destination);
                    DirectoryInfo infosSourceDir = new DirectoryInfo(source);
                   
                    save.copyFilesPartialSave(infosSourceDir, infosDestDir);
                }
                catch
                {
                    Console.WriteLine(Properties.Resources.error_directory_path);
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
                    Console.WriteLine(Properties.Resources.error_directory_path);
                }
            }
           
        }
    }
}