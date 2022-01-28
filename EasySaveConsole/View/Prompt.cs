using System;
using System.Globalization;
using System.Threading;
using EasySaveConsole.ViewModel;

namespace EasySaveConsole.View
{
    
    /// <summary>
    /// Display to user prompt to choose save filepath (input and output) and status when running
    /// </summary>
    public class Prompt
    {

        private MainViewModel mvm = new MainViewModel();

        private string[] arrayMainMenu;
        private string[] arrayMenuTraduction;
        private string[] arrayMenuShow;
        private string[] arrayMenuYesNo;
        private string[] arrayMenuExecuteSavingJob;

        public void MainMenu()
        {
            applyTrad();
            Console.WriteLine("-----Eassy Save Console v1.0-----");
            while (promptMainMenu(makeMenu(Properties.Resources.title_main_menu, arrayMainMenu))) ;
        }

        private bool promptMainMenu(string option)
        {
            bool keepTurning = true;

            switch (option)
            {
                case "0": // créer un travail de sauvegarde 
                    Console.Clear();
                    promptJobCreation(mvm.FetchSavingJob());
                    break;
                case "1": // ex�cute un travail de sauvegarde
                    while (promptExecuteSavingJob(makeMenu(Properties.Resources.execute_saving_job, arrayMenuExecuteSavingJob))) ;
                    break;
                case "2": // suprime un travail de sauvegarde
                    Console.Clear();
                    while (promptDeleteSavingJob(makeMenu(Properties.Resources.delete_saving_job, mvm.FetchSavingJob()), mvm.FetchSavingJob())) ;
                    break;
                case "3": // ouvre le journal des logs 
                    Console.Clear();
                    promptShowInfo(makeMenu(Properties.Resources.show_info, arrayMenuShow));
                    break;
                case "4": // change la langue
                    Console.Clear();
                    promptTraduction(makeMenu(Properties.Resources.change_lang, arrayMenuTraduction, new string[] { "fr", "en" }));
                    break;
                case "5":
                    Console.Clear();
                    promptShowSavingJob(mvm.FetchSavingJob());
                    break;
                case "x": // quitte l'app
                    keepTurning = false;
                    break;
            }
            return keepTurning;
        }

        private void promptJobCreation(string[] array)
        {
            int i = 0;
            string userInput = "";

            string[] userInputs = new string[4];
            // [0] job name
            // [1] 1source path
            // [2] destination path
            // [3] job type

            string[] queries =
            {
                Properties.Resources.enter_job_name,
                Properties.Resources.enter_source_path,
                Properties.Resources.enter_destination_path,
                Properties.Resources.enter_job_type
            };


            if (array.Length >= 5)
            {
                Console.WriteLine(Properties.Resources.impossible_create_saving_job);
            }
            else
            {
                Console.WriteLine(Properties.Resources.create_saving_job);
                while (i < 4 && userInput != "x")
                {
                    Console.WriteLine(queries[i]);
                    Console.WriteLine(("[x]  " + Properties.Resources.leave_current_menu));
                    userInput = Console.ReadLine();
                    if (userInput != "x")
                    {
                        userInputs[i] = userInput;
                    }
                    i++;
                }
                if (userInput != "x")
                {
                    mvm.CreateSavingJob(userInputs[0], userInputs[1], userInputs[2], userInputs[3]);
                }
            }
            Console.Clear();
        }

        private bool promptExecuteSavingJob(string option)
        {
            bool keepturning = false;

            switch (option)
            {
                case "0":
                    keepturning = promptExecuteAllSavingJob(makeMenu(Properties.Resources.confirm_execute_all_saving_job, arrayMenuYesNo, new string[] { "y", "n" }));
                    break;
                case "1":
                    keepturning = promptExecuteOneSavingJob(makeMenu(Properties.Resources.confirm_execute_all_saving_job, mvm.FetchSavingJob()), mvm.FetchSavingJob());
                    break;
                case "x":
                    break;
            }

            return keepturning;
        }

        private bool promptDeleteSavingJob(string option, string[] array)
        {
            bool keepTurning = false;
            string verif;
            string j;

            if (option != "x")
            {
                for (int i = 0; i < array.Length; i++)
                {
                    j = i.ToString();
                    if (option == j)
                    {
                        verif = makeMenu(Properties.Resources.confirm_delete, arrayMenuYesNo, new string[] { "y", "n" });

                        if (verif == "y")
                        {
                            mvm.DeleteSavingJob(array[i]);
                        }
                        else if (verif == "n")
                        {
                            keepTurning = true;
                        }

                    }
                }
            }
            return keepTurning;
        }

        private void promptShowInfo(string option)
        {
            switch (option)
            {
                case "0":
                    Console.WriteLine("afficher le journal des logs");
                    // appeller la fonction pour afficher les logs ici !
                    break;
                case "1":
                    Console.WriteLine("afficher l'etat d'avancement");
                    // appeller la fonction pour afficher l'etat d'avancment ici !
                    break;
                case "x":
                    break;
            }
        }

        private void promptTraduction(string option)
        {
            switch (option)
            {
                case "en":
                    mvm.Translate("en-US");
                    applyTrad();
                    break;
                case "fr":
                    mvm.Translate("fr-FR");
                    applyTrad();
                    break;
                case "x":
                    // quitte le menu traduction
                    break;
            }

        }

        private void promptShowSavingJob(string[] arrayName)
        {
            Console.WriteLine(Properties.Resources.list_jobs);

            for (int i = 0; i < arrayName.Length; i++)
            {
                Console.WriteLine(i + ".  " + arrayName[i]);
            }
        }

        private bool promptExecuteAllSavingJob(string option)
        {
            bool keepTurning = false;

            if (option == "y")
            {
                mvm.StartAllSavingJobs();
            }
            else if (option == "n")
            {
                keepTurning = true;
            }

            return keepTurning;
        }

        private bool promptExecuteOneSavingJob(string option, string[] array)
        {
            bool keepTurning = false;
            string j;

            if (option == "x")
            {
                keepTurning = true;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    j = i.ToString();
                    if (option == j)
                    {
                        mvm.StartSavingJob(array[i]);
                    }
                }
            }
            return keepTurning;
        }

        private string makeMenu(string title, string[] message)
        {
            bool keepTurning = true;
            int size = message.Length;
            string j;
            string result = "";

            while (keepTurning)
            {
                Console.WriteLine(title);

                // créer le menu 
                for (int i = 0; i < size; i++)
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
                        Console.Clear();
                        Console.WriteLine(Properties.Resources.user_input_error); // redémarre la boucle si la saisie est invalide 
                    }
                }
            }
            Console.Clear();
            return result;
        }

        private string makeMenu(string title, string[] message, string[] option)
        {
            bool keepTurning = true;
            int size = message.Length;
            string result = "";

            while (keepTurning)
            {
                Console.WriteLine(title);

                // créer le menu 
                while (keepTurning)
                {
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
                            Console.Clear();
                            Console.WriteLine(Properties.Resources.user_input_error); // redémarre la boucle si la saisie est invalide 
                        }
                    }
                }
               
            }
            Console.Clear();
            return result;
        }

        private void applyTrad()
        {
            arrayMainMenu = new string[] {
            Properties.Resources.create_saving_job,
            Properties.Resources.execute_saving_job,
            Properties.Resources.delete_saving_job,
            Properties.Resources.show_info,
            Properties.Resources.change_lang,
            Properties.Resources.show_job
            };
            arrayMenuTraduction = new string[]
            {
            Properties.Resources.change_lang_fr,
            Properties.Resources.change_lang_en
            };
            arrayMenuShow = new string[]
            {
            Properties.Resources.show_log ,
            Properties.Resources.show_state
            };
            arrayMenuYesNo = new string[]
            {
            Properties.Resources.yes,
            Properties.Resources.no
            };
            arrayMenuExecuteSavingJob = new string[]
            {
            Properties.Resources.execute_all_saving_job,
            Properties.Resources.execute_one_saving_job
            };
        }
    }
}

