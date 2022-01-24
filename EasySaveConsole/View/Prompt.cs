using EasySaveConsole.ViewModel;

namespace EasySaveConsole.View;

/**
 * Display to user prompt to choose save filepath (input and output) and status when running
 */
public class Prompt
{
    private string sourcePath;
    private string destinationPath;
    private string name;

    private MainViewModel mvm = new MainViewModel();
    public void Start()
    {
        // Ask for create a job or start a new one

        promptJobCreation();

        promptJobSelection();
    }

    private void promptJobCreation()
    {
        // Create saving job
        // Ask name
        // Ask paths 
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