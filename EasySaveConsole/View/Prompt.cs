using System;
using System.IO;
using EasySaveConsole.ViewModel;

namespace EasySaveConsole.View
{

    /**
     * Display to user prompt to choose save filepath (input and output) and status when running
     */
    public class Prompt
    {
        public string sourcePath;
        public string destinationPath;
        public string name;

        private MainViewModel mvm = new MainViewModel();
        public void Start()

        {
            promptJobCreation();

            promptJobSelection();
        }

        private void promptJobCreation()
        {

            Console.WriteLine("Enter the name of you save");
            name = Console.ReadLine();
            Console.WriteLine("Enter save source path:");
            sourcePath = Console.ReadLine();
            Console.WriteLine("Enter save destination path:");
            destinationPath = Console.ReadLine();
           

            // Ask if run the job now
            
            // Call ViewModel
            mvm.CreateSavingJob(name, sourcePath, destinationPath);
        }

        private void promptJobSelection()
        {
            // Run existing job(s) by asking their names
            
        }
    }
}
