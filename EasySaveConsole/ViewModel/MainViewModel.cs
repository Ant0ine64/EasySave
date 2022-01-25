using EasySaveConsole.Model;

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

        public void CreateSavingJob(string name, string source, string destination, string status="TODO")
        {
            job.Name = name;
            job.SourcePath = source;
            job.DestinationPath = destination;
            job.Status = status;
            Job.Add(job);
        }
    }
}