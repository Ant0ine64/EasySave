using System.IO;

namespace EasySaveConsole.Model
{

    public class Save
    {
        public void copyFiles(string source, string destination)
        {
            string[] files = Directory.GetFiles(source);

            foreach (string f in files)
            {
                // Remove path from the file name.
                string fName = f.Substring(source.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(source, fName), Path.Combine(destination, fName), true);
            }
        }
    }
}