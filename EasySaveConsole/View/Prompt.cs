using System;
using System.Globalization;
using System.Threading;
using EasySaveConsole.ViewModel;
using System;

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

        private string[] arrayMainMenu = 
        {   
            Properties.Resources.create_saving_job, 
            Properties.Resources.execute_saving_job, 
            Properties.Resources.delete_saving_job, 
            Properties.Resources.show_info, 
            Properties.Resources.change_lang 
        };
        private string[] arrayMenuTraduction = 
        { 
            Properties.Resources.change_lang_fr,
            Properties.Resources.change_lang_en 
        };

        public void MainMenu()
        {
            //test();
            promptMainMenu(makeMenu(arrayMainMenu));
            /*promptTraduction(makeMenu(arrayMenuTraduction,new string[]{ "fr", "en"}));
             promptJobCreation(); //0
             promptExecuteSavingJob(); //1
             promptDeleteSavingJob(); //2
             promptShowInfo(); //3
             promptTraduction(); //4*/

        }

        private void promptMainMenu(string option)
        {
            
            switch(option)
            {
                case "0":
                    promptJobCreation();
                    break;
                case "1":
                    promptExecuteSavingJob(); 
                    break;
                case "2":
                    promptDeleteSavingJob();
                    break;
                case "3":
                    promptShowInfo();
                    break;
                case "4":
                    promptTraduction(makeMenu(arrayMenuTraduction, new string[] { "fr", "en" }));
                    break;
                case "x":
                    // quitte l'app
                    break;
            }
        }

        private void promptJobCreation()
        {
            string jobName;
            string sourcePath;
            string destinationPath;
            string jobType;

            Console.WriteLine(Properties.Resources.enter_job_name);
            jobName = Console.ReadLine();
            Console.WriteLine(Properties.Resources.enter_source_path);
            sourcePath = Console.ReadLine();
            Console.WriteLine(Properties.Resources.enter_destination_path);
            destinationPath = Console.ReadLine();
            Console.WriteLine(Properties.Resources.enter_job_type);
            jobType = Console.ReadLine();

            // Call ViewModel
            mvm.CreateSavingJob(jobName, sourcePath, destinationPath, jobType);
        }

        private void promptExecuteSavingJob()
        {
            // Run existing job(s) by asking their names           
        }

        private void promptDeleteSavingJob()
        {
            // lance la recherche des jobs 
            // demande à l'utilisateur lequel il veut supprimer
        }

        private void promptShowInfo()
        {
            // demande à l'utilisateur si il veut voir les logs ou l'état de la sauvegarde
        }

        private void promptTraduction(string option)
        {
            switch (option)
            {
                case "en":
                    mvm.translate("en-US");
                    break;
                case "fr":
                    mvm.translate("fr-FR");
                    break;
                case "x":
                    // quitte le menu traduction
                    break;
            }
            promptMainMenu(makeMenu(Properties.Resources.title_main_menu,arrayMainMenu)); // reviens au menu principal
        }

        private string makeMenu(string title, string[] message)
        {
            bool keepTurning = true;
            int size = message.Length;
            string j;
            string result="";

            Console.WriteLine(title);

            while (keepTurning)
            {
                // créer le menu 
                for (int i=0; i < size; i++) 
                {
                    Console.WriteLine("[" + i + "]  " + message[i]);
                }
                Console.WriteLine("[x]  " + Properties.Resources.leave_current_menu);

                
                result = Console.ReadLine();

                // vérifie le choix de l'utilisateur
                if (result == "x")
                {
                    keepTurning = false; // quitte la boucle
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        j = i.ToString();
                        if (result == j)
                        {
                            keepTurning = false;
                        }

                    }
                    if (keepTurning == true)
                    {
                        Console.WriteLine(Properties.Resources.user_input_error); // redémarre la boucle si la saisie est invalide 
                    }
                }
            }
            return result;
        }

        private string makeMenu(string title, string[] message, string[] option)
        {
            bool keepTurning = true;
            int size = message.Length;
            string result = "";

            Console.WriteLine(title);

            while (keepTurning)
            {
                // créer le menu 
                for (int i = 0; i < size; i++)
                {
                    Console.WriteLine("[" + option[i] + "]  " + message[i]);
                }
                Console.WriteLine("[x]  " + Properties.Resources.leave_current_menu);

                
                result = Console.ReadLine();

                // vérifie le choix de l'utilisateur
                if (result == "x")
                {
                    keepTurning = false; // quitte la boucle
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (result == option[i])
                        {
                            keepTurning = false;
                        }

                    }
                    if (keepTurning == true)
                    {
                        Console.WriteLine(Properties.Resources.user_input_error); // redémarre la boucle si la saisie est invalide 
                    }
                }
            }
            return result;
        }


        private void test()
        {
            Console.WriteLine("test");
        }
        private void test(string num)
        {
            Console.WriteLine("test "+ num);
        }

    }
}