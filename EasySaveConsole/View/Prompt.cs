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

        private string[] arrayMainMenu; // contains the texts of the main menu options 
        private string[] arrayMenuTraduction; // contains the texts of the translation menu options 
        private string[] arrayMenuLogFormat; // contains the texts of the logbook menu options 
        private string[] arrayMenuYesNo; // contains the texts of the Confirmation menu options 
        private string[] arrayMenuExecuteSavingJob; // contains the texts of the execute saving job menu options 

        /// <summary>
        /// run the UI
        /// </summary>
        public void MainMenu() 
        {
            applyTrad();
            Console.WriteLine("-----Easy Save Console v1.1-----");
            while (promptMainMenu(makeMenu(Properties.Resources.title_main_menu, arrayMainMenu)));
        }

        /// <summary>
        /// executes one of the function of "Main Menu" corresponding to the user input
        /// </summary>
        /// <param name="option">option is the user input</param>
        /// <returns></returns>
        private bool promptMainMenu(string option)
        {
            bool keepTurning = true;

            switch (option)
            {
                case "0": // shows saving jobs
                    Console.Clear();
                    promptShowSavingJob(mvm.FetchSavingJob());
                    break;                    
                case "1": // creates a saving job
                    Console.Clear();
                    promptJobCreation(mvm.FetchSavingJob());
                    break;
                case "2": // runs a saving job
                    while (promptExecuteSavingJob(makeMenu(Properties.Resources.execute_saving_job, arrayMenuExecuteSavingJob))) ;
                    break;
                case "3": // delete logBook 
                    Console.Clear();
                    while (promptDeleteSavingJob(makeMenu(Properties.Resources.delete_saving_job, mvm.FetchSavingJob()), mvm.FetchSavingJob())) ;
                    break;
                case "4": // chooses log file format (.xml /.json)
                    Console.Clear();
                    promptChangeLogFormat(makeMenu(Properties.Resources.change_log_format, arrayMenuLogFormat, new string[] { "json", "xml" }));
                    break;
                case "5": // changes languages
                    Console.Clear();
                    promptTraduction(makeMenu(Properties.Resources.change_lang, arrayMenuTraduction, new string[] { "fr", "en" }));
                    break;
                case "x": // leaves the App
                    keepTurning = false;
                    break;
            }
            return keepTurning;
        }

        /// <summary>
        ///  asks user for the needed information  to create a saving job
        /// </summary>
        /// <param name="arrayJobName">arrayJobName is an array containing all names of saving jobs</param>
        private void promptJobCreation(string[] arrayJobName)
        {
            int i = 0;
            string userInput = "";

            string[] userInputs = new string[4];
            // [0] job name
            // [1] source path
            // [2] destination path
            // [3] job type

            string[] queries =
            {
                Properties.Resources.enter_job_name,
                Properties.Resources.enter_source_path,
                Properties.Resources.enter_destination_path,
                Properties.Resources.enter_job_type
            };


            if (arrayJobName.Length >= 5)
            {
                Console.WriteLine(Properties.Resources.impossible_create_saving_job); // show an error if 5 saving job exist
            }
            else
            {
                Console.WriteLine(Properties.Resources.create_saving_job);
                while (i < 4 && userInput != "x")
                {
                    Console.WriteLine(queries[i]); // show the informations that the user must enter
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

        /// <summary>
        /// executes one of the function of "Executes Saving Job Menu" corresponding to the user input
        /// </summary>
        /// <param name="option">option is the user input</param>
        /// <returns>return a bool to verif if the user stays on the "Executes Saving Job Menu" or not</returns>
        private bool promptExecuteSavingJob(string option)
        {
            bool keepturning = false;

            switch (option)
            {
                case "0":
                    keepturning = promptExecuteAllSavingJob(makeMenu(Properties.Resources.confirm_execute_all_saving_job, arrayMenuYesNo, new string[] { "y", "n" }));  // ask the user if he is sure he wants to execute all saving job
                    break;
                case "1":
                    keepturning = promptExecuteOneSavingJob(makeMenu(Properties.Resources.confirm_execute_all_saving_job, mvm.FetchSavingJob()), mvm.FetchSavingJob()); // ask the user what work he wants to delete
                    break;
                case "x":
                    break;
            }

            return keepturning;
        }

        /// <summary>
        /// executes one of the function of "Deletes Saving Job Menu" corresponding to the user input 
        /// </summary>
        /// <param name="option">option is the user input</param>
        /// <param name="arrayJobName">arrayJobName is an array containing all names of saving jobs</param>
        /// <returns>return a bool to verif if the user stays on the "Deletes Saving Job Menu" or not</returns>
        private bool promptDeleteSavingJob(string option, string[] arrayJobName)
        {
            bool keepTurning = false;
            string verif;

            if (option != "x")
            {
                for (int i = 0; i < arrayJobName.Length; i++)
                {
                    if (option == i.ToString())
                    {
                        verif = makeMenu(Properties.Resources.confirm_delete, arrayMenuYesNo, new string[] { "y", "n" }); // ask the user if he is sure he wants to delete it 

                        if (verif == "y") 
                        {
                            mvm.DeleteSavingJob(arrayJobName[i]); // delete the Saving Job [i]
                        }
                        else if (verif == "n")
                        {
                            keepTurning = true; // stays on menu "Deletes Saving Job Menu"
                        }

                    }
                }
            }
            return keepTurning; // return a bool to verif if the user stays on the "Deletes Saving Job Menu" or not
        }

        /// <summary>
        /// changes the format of log files (xml or json)
        /// </summary>
        /// <param name="option">option is the user input</param>
        private void promptChangeLogFormat(string option)
        {
            string logFormat = "";

            if (option != "x")
            {          
                logFormat = option;
                mvm.SelectLogFormat(logFormat);
                Console.Clear();
                Console.WriteLine(Properties.Resources.log_file_path + mvm.returnLogFilePath(logFormat));
            }       
        }

        /// <summary>
        /// executes one of the function of "Choose Language Menu" corresponding to the user input
        /// </summary>
        /// <param name="option">option is the user input</param>
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
                    // leaves the translate menu
                    break;
            }

        }

        /// <summary>
        /// show the existing saving job
        /// </summary>
        /// <param name="arrayJobName">arrayJobName is an array containing all names of saving jobs</param>
        private void promptShowSavingJob(string[] arrayJobName)
        {
            if (arrayJobName.Length != 0)
            {
                Console.WriteLine(Properties.Resources.list_jobs);
                for (int i = 0; i < arrayJobName.Length; i++)
                {
                    Console.WriteLine(i + ".  " + arrayJobName[i]);
                }
            }
            else
            {
                Console.WriteLine(Properties.Resources.no_saving_job);
            }                
        }

        /// <summary>
        /// executes one of the function of "execute all saving jobs Menu" corresponding to the user input
        /// </summary>
        /// <param name="option">option is the user input</param>
        /// <returns>return a bool to verif if the user stays on the "execute all saving jobs Menu" or not</returns>
        private bool promptExecuteAllSavingJob(string option)
        {
            bool keepTurning = false;

            if (option == "y")
            {
                mvm.StartAllSavingJobs();
            }
            else if (option == "n")
            {
                keepTurning = true; // stay on the "execute all saving jobs Menu"
            }

            return keepTurning; // return a bool to verif if the user stays on the "execute all saving jobs Menu" or not
        }

        /// <summary>
        /// executes one of the function of "execute one saving jobs Menu" corresponding to the user input
        /// </summary>
        /// <param name="option">option is the user input</param>
        /// <returns>return a bool to verif if the user stays on the "execute one saving jobs Menu" or not</returns>
        private bool promptExecuteOneSavingJob(string option, string[] arrayJobName)
        {
            bool keepTurning = false;

            if (option == "x")
            {
                keepTurning = true; // stay on the "execute one saving jobs Menu"
            }
            else
            {
                for (int i = 0; i < arrayJobName.Length; i++)
                {
                    if (option == i.ToString())
                    {
                        mvm.StartSavingJob(arrayJobName[i]);
                    }
                }
            }
            return keepTurning; //return a bool to verif if the user stays on the "execute one saving jobs Menu" or not
        }

        /// <summary>
        /// create and show a menu with the data set in parameters, and retrieves the user input
        /// </summary>
        /// <param name="title">title of the menu</param>
        /// <param name="message">array containing descriptions of the options that the user can choose  containing</param>
        /// <returns>return the user input</returns>
        private string makeMenu(string title, string[] message)
        {
            bool keepTurning = true;
            int size = message.Length;
            string result = "";

            while (keepTurning)
            {
                Console.WriteLine(title); // show the title

                // create and show the menu
                for (int i = 0; i < size; i++)
                {
                    Console.WriteLine("[" + i + "]  " + message[i]);
                }
                Console.WriteLine("[x]  " + Properties.Resources.leave_current_menu);


                result = Console.ReadLine();

                // check the user input
                if (result == "x")
                {
                    keepTurning = false; // leave the loop while
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (result == i.ToString()) // checks if the user input is valid 
                        {
                            keepTurning = false; // leave the loop while
                        }

                    }
                    if (keepTurning == true)
                    {
                        Console.Clear();
                        Console.WriteLine(Properties.Resources.user_input_error); // restarts the loop if the user input is invalid and shows error message
                    }
                }
            }
            Console.Clear();
            return result; // return user input
        }
        /// <summary>
        /// create and show a menu with the data set in parameters, and retrieves the user input
        /// </summary>
        /// <param name="title">title of the menu</param>
        /// <param name="message">array containing descriptions of the options that the user can enter</param>
        /// <param name="option">array containing the options that the user can enter</param>
        /// <returns>return user input</returns>
        private string makeMenu(string title, string[] message, string[] option)
        {
            bool keepTurning = true;
            int size = message.Length;
            string result = "";

            while (keepTurning)
            {
                Console.WriteLine(title);

                // create and show the menu 
                while (keepTurning)
                {
                    for (int i = 0; i < size; i++)
                    {
                        Console.WriteLine("[" + option[i] + "]  " + message[i]);
                    }
                    Console.WriteLine("[x]  " + Properties.Resources.leave_current_menu);


                    result = Console.ReadLine();

                    // check the user input
                    if (result == "x")
                    {
                        keepTurning = false; // leave the loop while
                    }
                    else
                    {
                        for (int i = 0; i < size; i++)
                        {
                            if (result == option[i]) // checks if the user input is valid 
                            {
                                keepTurning = false; // leave the loop while
                            }

                        }
                        if (keepTurning == true)
                        {
                            Console.Clear();
                            Console.WriteLine(Properties.Resources.user_input_error); // restarts the loop if the user input is invalid and shows error message
                        }
                    }
                }
               
            }
            Console.Clear();
            return result; // return user input
        }
        /// <summary>
        /// applies the translation to the arrays of option descriptions 
        /// </summary>
        private void applyTrad()
        {
            arrayMainMenu = new string[] {
            Properties.Resources.show_saving_jobs,
            Properties.Resources.create_saving_job,
            Properties.Resources.execute_saving_job,
            Properties.Resources.delete_saving_job,
            Properties.Resources.change_log_format,
            Properties.Resources.change_lang
            };
            arrayMenuTraduction = new string[]
            {
            Properties.Resources.change_lang_fr,
            Properties.Resources.change_lang_en
            };
            arrayMenuLogFormat = new string[]
            {
            Properties.Resources.json_log_format,
            Properties.Resources.xml_log_format
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

