using EasySaveConsole.Model;
using System.IO;

namespace EasySaveConsole.ViewModel
{

    public class MainViewModel
    {
        private Save save = new Save();
        private Job job = new Job();

        public void StartSavingJob(string jobName)
        {
            save.copyFiles(job.source, job.destination);

            // write log file
        }

        public void CreateSavingJob(string name, string source, string destination)
        {
           
        }
    }
}