using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace TestLauncher.MainScripts
{
    public class SaveManager
    {
        private static readonly string LocalPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);

        public static void Save(UserSettings settings) 
        {
            string path = Path.Combine(LocalPath, LauncherConst.SettingsFilePath());

            string directoriPath = Path.Combine(LocalPath, LauncherConst.COMPANY_NAME);
            if (!Directory.Exists(directoriPath))
                Directory.CreateDirectory(directoriPath);

            string jsonFile = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path, jsonFile);
        }
        public static UserSettings LoadUserSettings()
        {

            string path = Path.Combine(LocalPath, LauncherConst.SettingsFilePath());

            if (!File.Exists(path))
                return new UserSettings();

            try
            {
                string jsonFile = File.ReadAllText(path);
                UserSettings userSettings = JsonConvert.DeserializeObject<UserSettings>(jsonFile);
                return userSettings;
            }
            catch (Exception e)
            {
                //new Exeption(e.Message);
                MessageBox.Show("Failed to deserialize json! Message: " + e.Message);
                return new UserSettings(); 
            }
        }
    }
}
