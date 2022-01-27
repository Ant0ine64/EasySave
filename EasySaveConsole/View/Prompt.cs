using EasySaveConsole.ViewModel;
using System;

namespace EasySaveConsole.View
{

    /**
     * Display to user prompt to choose save filepath (input and output) and status when running
     */
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
            while(promptMainMenu(makeMenu(Properties.Resources.title_main_menu, arrayMainMenu)));
        }

        private bool promptMainMenu(string option)
        {
            bool keepTurning = true;

            switch(option)
            {
                case "0": // créer un travail de sauvegarde 
                    promptJobCreation(mvm.fetchSavingJob());
                    break;
                case "1": // exécute un travail de sauvegarde
                    while(promptExecuteSavingJob(makeMenu(Properties.Resources.execute_saving_job, arrayMenuExecuteSavingJob))); 
                    break;
                case "2": // suprime un travail de sauvegarde
                    while(promptDeleteSavingJob(makeMenu(Properties.Resources.delete_saving_job,mvm.fetchSavingJob()), mvm.fetchSavingJob()));
                    break;
                case "3": // ouvre le journal des logs 
                    promptShowInfo(makeMenu(Properties.Resources.show_info, arrayMenuShow));
                    break;
                case "4": // change la langue
                    promptTraduction(makeMenu(Properties.Resources.change_lang, arrayMenuTraduction, new string[] { "fr", "en" }));
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
            string userInput="";

            string[] userInputs = 
            {
                "", // job name
                "", // source path
                "", // destination path
                ""  // job type
            };
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
                while (i<4 && userInput!="x")
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
        }

        private bool promptExecuteSavingJob(string option)
        {
            bool keepturning = false;

            switch(option)
            {
                case "0":                    
                    keepturning = promptExecuteAllSavingJob(makeMenu(Properties.Resources.confirm_execute_saving_job, arrayMenuYesNo, new string[] { "y", "n" }));
                    break;
                case "1":
                    keepturning = promptExecuteOneSavingJob(makeMenu(Properties.Resources.confirm_execute_saving_job, mvm.fetchSavingJob()), mvm.fetchSavingJob());
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
                for(int i = 0; i < array.Length; i++)
                {
                    j = i.ToString();
                    if (option == j)
                    {
                        verif = makeMenu(Properties.Resources.confirm_delete, arrayMenuYesNo, new string[] { "y", "n" });

                        if (verif =="y")
                        {
                            mvm.deleteSavingJob(array[i]);
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
                    mvm.translate("en-US");
                    applyTrad();
                    break;
                case "fr":
                    mvm.translate("fr-FR");
                    applyTrad();
                    break;
                case "x":
                    // quitte le menu traduction
                    break;
            }
            
        }

        private bool promptExecuteAllSavingJob(string option)
        {
            bool keepTurning = false;

            if (option == "y")
            {
                mvm.executeSavingJob();
            }
            else if (option == "n")
            {
                keepTurning = true;
            }

            return keepTurning;
        }

        private bool promptExecuteOneSavingJob(string option,string[] array)
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
                        mvm.executeSavingJob(array[i]);
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

        private void applyTrad()
        {
            arrayMainMenu = new string[] {
                Properties.Resources.create_saving_job,
                Properties.Resources.execute_saving_job,
                Properties.Resources.delete_saving_job,
                Properties.Resources.show_info,
                Properties.Resources.change_lang
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