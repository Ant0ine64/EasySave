using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EasySaveConsole.Model
{
    public sealed class CryptoSoft
    {
        private static CryptoSoft _instance;
        
        public string Key;
        public string CryptoSoftPath;

        public void XorCypher(string source, string destination)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = CryptoSoftPath;
                process.StartInfo.Arguments = $"\"{source}\" \"{destination}\" {Key}";
                process.EnableRaisingEvents = true;
                process.Start();
                process.WaitForExit(60000);
            }
            
        }

        public static CryptoSoft GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CryptoSoft();
            }
            return _instance;
        }
    }
}