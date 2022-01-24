using EasySaveConsole.ViewModel;

namespace EasySaveConsole.View;

/**
 * Display to user prompt to choose save filepath (input and output) and status when running
 */
public class Prompt
{
    private string sourcePath;
    private string destinationPath;

    private MainViewModel mvm = new MainViewModel();
    public void Start()
    {
        // Ask paths 
        Console.WriteLine("Enter save source path:");
        sourcePath = Console.ReadLine();
        Console.WriteLine("Enter save destination path:");
        destinationPath = Console.ReadLine();
        
        // Call ViewModel
        mvm.StartSavingJob(sourcePath, destinationPath);
        
        // Optional : Display live status
    }
}