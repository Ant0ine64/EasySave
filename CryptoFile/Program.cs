using System;
using System.IO;
using System.Security;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoFile
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                // Define XOR key
                string base64Key = args[2];
                string key = SanitizeBase64(base64Key);
                byte[] bytesKey = Convert.FromBase64String(key);
                // File to encrypt or encrypt
                string inputFile = args[0]; 
                // File to decrypt or decrypt
                string outputFile = args[1];

                // Console.WriteLine("chiffrement/déchiffrement en cours");
                EncryptFile(inputFile, outputFile, bytesKey);
                // Console.WriteLine("chiffrement/déchiffrement terminé");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.Error.WriteLine($"CryptoSoft : Not enough args provided\n{e}");
                Environment.ExitCode = 1;
            }
        }

        private static string SanitizeBase64(string input)
        {
            // add padding if the string length does not comply with base64 encoding
            input = (input.Length % 4) switch
            {
                // cheat beacause not a valid base 64 string so add one more to key and reprocces
                1 => SanitizeBase64(input + "a"),
                // add padding
                2 => input + new string('=', 2),
                3 => input + new string('=', 1),
                // padding is good
                _ => input
            };
            
            // replace - by + and _ by /
            return input.Replace('-', '+').Replace('_', '/');
        }

        private static void EncryptFile(string inputFile,string outputFile,byte[] key)
        {
            FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read); // ouvre le fichier d'entré
            FileStream fsOutput = new FileStream(outputFile, FileMode.Create,FileAccess.Write); // créer le fichier de sortie

            long length = fsInput.Length;
            
            // Input and output bytes arrays
            byte[] input = new byte[length]; 
            byte[] output = new byte[length];
            // Fill input byte array
            fsInput.Read(input, 0, (int) length);

            int indexKey = 0;
            for (int i=0 ; i < input.Length ; i++)
            {
                output[i] = (byte)(input[i] ^ key[indexKey]); // opération Xor sur un byte de la clé et un byte du fichier d'entré 

                if (indexKey == key.Length-1) // (40 - 43) répête la clé autant de fois que nécessaire pour chiffrer le fichier d'entre
                    indexKey = 0;
                else
                    indexKey++;
            }

            // Write bytes to ouput file
            fsOutput.Write(output);

            fsInput.Close(); // ferme le fichier d'entré
            fsOutput.Close(); // ferme le fichier de sortie 
        }
    }
}
