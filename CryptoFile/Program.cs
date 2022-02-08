using System;
using System.IO;
using System.Security;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace CryptoFile
{
    class Program
    {
        
        static void Main(string[] args)
        {

            string key = args[2]; // définie la clé de chiffrage/déchiffrage
            string inputFile = args[0]; // défini le chemin du fichier d'entre 
            string outputFile = args[1]; // défni le chemin du fichier de sortie

            Console.WriteLine("chiffrement/déchiffrement en cours");
            EncryptFile(inputFile, outputFile, key);
            Console.WriteLine("chiffrement/déchiffrement terminé");
        }

        static void EncryptFile(string inputFile,string outputFile,string Key)
        {
            FileStream FS_Input = new FileStream(inputFile, FileMode.Open, FileAccess.Read); // ouvre le fichier d'entré
            FileStream FS_Output = new FileStream(outputFile, FileMode.Create,FileAccess.Write); // créer le fichier de sortie
            
            byte[] byteArrayKey = ASCIIEncoding.ASCII.GetBytes(Key); // créer un tableau d'octet à partir de la clé
            byte[] byteArrayInput = new byte[FS_Input.Length]; // tableau qui va contenir tout les octet du fichier d'entre
            FS_Input.Read(byteArrayInput, 0, byteArrayInput.Length); // remplie un tableau d'octet avec les octets du fichier d'entré 
            byte[] byteArrayOutput = new byte[FS_Input.Length]; // créer un tableau d'octet à partir de la taille du fichier d'origine

            int indexKey = 0;
            for (int i=0 ; i < byteArrayInput.Length ; i++)
            {
                byteArrayOutput[i] = (byte)(byteArrayInput[i] ^ byteArrayKey[indexKey]); // opération Xor sur un byte de la clé et un byte du fichier d'entré 

                if (indexKey == byteArrayKey.Length-1) // (40 - 43) répête la clé autant de fois que nécessaire pour chiffrer le fichier d'entre
                    indexKey = 0;
                else
                    indexKey++;
            }

            FS_Output.Write(byteArrayOutput); // écrit dans le nouveau fichier

            FS_Input.Close(); // ferme le fichier d'entré
            FS_Output.Close(); // ferme le fichier de sortie 
        }
    }
}
