using System;
using System.IO;
using System.Linq;

namespace EasySaveConsole.Model
{

    public class Save
    {
        public void copyFilesPartialSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir)// sauvegarde partielle
        {
       
            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();
          
            foreach (FileInfo infosSourceFile in infosSourceFiles)
            {
                foreach (FileInfo infosDestinationFile in infosDestinationFiles)
                {
                  
                    // si le fichier a été modifié (nom est le meme mais pas l'heure de modif)
                    if (infosSourceFile.Name == infosDestinationFile.Name && infosSourceFile.LastWriteTime != infosDestinationFile.LastWriteTime)
                    {
                        // copie en écrasant la version existante  
                            File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                            Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);                      
                    }
                   

                }
                // si le fichier n'existe pas dans le destination on le copie
                if (infosDestinationFiles.Any(x => x.Name == infosSourceFile.Name)) { }
                else
                {
                    
                        File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                        Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);
                    
                }
            }
           
        }

        public void copyFilesEntireSave(DirectoryInfo infosSourceDir, DirectoryInfo infosDestDir)// Sauvegarde entière
        {

            FileInfo[] infosDestinationFiles = infosDestDir.GetFiles();
            FileInfo[] infosSourceFiles = infosSourceDir.GetFiles();

            foreach (FileInfo infosDestinationFile in infosDestinationFiles)// clean du dossier de destination
            {
                infosDestinationFile.Delete();              

            }

            foreach (FileInfo infosSourceFile in infosSourceFiles)// copie de tous les fichiers du sorce vers le destination
            {    
                    File.Copy(Path.Combine(infosSourceDir.FullName, infosSourceFile.Name), Path.Combine(infosDestDir.FullName, infosSourceFile.Name), true);
                    Console.WriteLine(Properties.Resources.file_transfered + infosSourceFile.Name);         
            }
        }
    }
}