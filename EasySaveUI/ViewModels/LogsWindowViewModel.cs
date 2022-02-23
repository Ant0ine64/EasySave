using System.Collections.ObjectModel;
using ReactiveUI;
using System.Windows.Input;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using EasySaveUI.Views;

namespace EasySaveUI.ViewModels
{
    public class LogsWindowViewModel : ViewModelBase
    {
        private string directoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "easysave");

        public ObservableCollection<string> Logs { get; private set; }

        public ICommand ButtonLog { get; set; }

        public ICommand ButtonBack { get; set; }


        public LogsWindowViewModel()
        {
            Logs = new ObservableCollection<string>(fetchLogFiles());

            ButtonLog = ReactiveCommand.Create((string? arg) => 
            {
                openLogFile(arg);
            });

            ButtonBack = ReactiveCommand.Create(() =>
            {
                MainWindow mainWindow = new MainWindow();
                updateContent(mainWindow.Content, mainWindow.DataContext);
                mainWindow.Close();
            });
        }


        private List<string> fetchLogFiles()
        {            
            List<string> logFiles = new List<string>();
            try
            {
                logFiles = Directory.GetFiles(directoryPath, "*_log.*").ToList();
            }
            catch (Exception e) { }

            List<string> logFileNames = new List<string>();
            foreach(var File in logFiles)
            {
                logFileNames.Add(Path.GetFileName(File));
            }
            return logFileNames;
        }
        /// <summary>
        /// opens the selected file with the default application
        /// </summary>
        /// <param name="fileName">Name of selected File</param>
        private void openLogFile(string fileName)
        {
            string file = directoryPath + "\\" + fileName;
            Process openLog = new Process();
            openLog.StartInfo = new ProcessStartInfo(file)
            {
                UseShellExecute = true
            };
            openLog.Start();
        }

    }
}
