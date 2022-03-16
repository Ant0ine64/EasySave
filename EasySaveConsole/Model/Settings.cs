using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EasySaveConsole.Properties;

namespace EasySaveConsole.Model
{
    public class Settings : ModelBase
    {
        public string CryptoSoftPath { get; set; } = "";
        public string Lang { get; set; } = "fr-FR";
        public string LogFormat { get; set; } = "json";
        public string CryptoKey { get; set; } = "";
        public static Settings Instance { get; set; }
        private List<string> blockingApp = new List<string>();
        public List<string> BlockingApp
        {
            get =>
                blockingApp;
            set =>
                SetField(ref blockingApp,
                    value,
                    nameof(BlockingApp));
        }

        public static string SettingsFile;

        public Settings() {}
        public Settings(bool create)
        {
            if (create)
            {
                CreateFile();
            }           
        }

        private static void CreateFile()
        {
            var settingsDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "easysave");
            SettingsFile = Path.Join(settingsDirectory, "settings.json");
            try
            {
                // create directory temp if not exists
                if (!Directory.Exists(settingsDirectory))
                    Directory.CreateDirectory(settingsDirectory);
                // create empty log.txt file to start with
                if (!File.Exists(SettingsFile))
                    File.WriteAllText(SettingsFile, createBaseContent());
            }
            catch
            {
                Console.WriteLine("Settings: " + Resources.perm_error);
            }
        }

        private void CreateCryptoKey()
        {
            string charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$%#*.!?:-@=(){}[]_";
            char[] charsOfKey = new char[16];
            var random = new Random();

            for (int i = 0; i < charsOfKey.Length; i++)
            {
                charsOfKey[i] = charList[random.Next(charList.Length)];
            }
            CryptoKey = new String(charsOfKey);
        }

        public void Write()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }

        private static string createBaseContent()
        {
            var settings = new Settings();
            settings.CreateCryptoKey();
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions {WriteIndented = true});
            return json;
        }

        public void ReadSettings()
        {
            string jsonFile = File.ReadAllText(SettingsFile);
            var settings = JsonSerializer.Deserialize<Settings>(jsonFile);
            
            // assign values read to the instance
            CryptoSoftPath = settings.CryptoSoftPath;
            Lang = settings.Lang;
            LogFormat = settings.LogFormat;
            CryptoKey = settings.CryptoKey;
            BlockingApp = settings.BlockingApp;
        }
    }
}