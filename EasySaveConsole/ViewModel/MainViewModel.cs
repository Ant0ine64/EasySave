using EasySaveConsole.Model;
using System.Globalization;
using System.Threading;

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

        public void CreateSavingJob(string name, string source, string destination, string type)
        {
            //Save
            //Write state file 

        }

        public void translate(string lang)
        {
            CultureInfo ui_culture = new CultureInfo(lang);
            CultureInfo culture = new CultureInfo(lang);

            Thread.CurrentThread.CurrentUICulture = ui_culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public string[] fetchSavingJob()
        {
            // retourne un tableau contenant le nom des travaux
            return null;
        }

        public void deleteSavingJob(string name)
        {
            // supprime le travail de sauvegarde à partir du nom
        }

        public void executeSavingJob()
        {
            //execute tous les travail de sauvegarde
        }

        public void executeSavingJob(string name)
        {
            //execute le travail de sauvegarde à partir du nom 
        }
    }
}