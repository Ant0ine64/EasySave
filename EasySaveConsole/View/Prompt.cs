using System;
using System.IO;
using System.Globalization;
using System.Threading;
using EasySaveConsole.ViewModel;

namespace EasySaveConsole.View
{

    /**
     * Display to user prompt to choose save filepath (input and output) and status when running
     */
    public class Prompt
    {
        public string SourcePath;
        public string DestinationPath;
        public string Name;
        public string Type;

        private MainViewModel mvm = new MainViewModel();
        public void Start()

        {
       

            Console.WriteLine(Properties.Resources.current_lang);
            if (Console.ReadLine() == "Y") { 
                if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr")
                {
                    CultureInfo ui_culture = new CultureInfo("en-US");
                    CultureInfo culture = new CultureInfo("en-US");

                    Thread.CurrentThread.CurrentUICulture = ui_culture;
                    Thread.CurrentThread.CurrentCulture = culture;
                }else
                {
                    CultureInfo ui_culture = new CultureInfo("fr-FR");
                    CultureInfo culture = new CultureInfo("fr-FR");

                    Thread.CurrentThread.CurrentUICulture = ui_culture;
                    Thread.CurrentThread.CurrentCulture = culture;
                }
           
            }
            else { }
            // Ask for create a job or start a new one

            promptJobCreation();

            promptJobSelection();
        }

        private void promptJobCreation()
        {
            // Create saving job
            // Ask name
            Console.WriteLine(Properties.Resources.enter_nameSave);
            Name = Console.ReadLine();
            Console.WriteLine(Properties.Resources.enter_source);
            SourcePath = Console.ReadLine();
            Console.WriteLine(Properties.Resources.enter_destination);
            DestinationPath = Console.ReadLine();
           
            Console.WriteLine(Properties.Resources.type_save_question);
            Type = Console.ReadLine();
            // Ask if run the job now
            
            // Call ViewModel
            mvm.CreateSavingJob(Name, SourcePath, DestinationPath, Type);
        }

        private void promptJobSelection()
        {
            // Run existing job(s) by asking their names
            
        }
    }
}
