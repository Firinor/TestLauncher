using System;
using System.IO;

namespace TestLauncher.MainScripts
{
    public class SaveManager
    {
        private static readonly string LocalPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);

        public static void Save(UserSettings settings) 
        {
            string path = Path.Combine(LocalPath, LauncherSettings.FullSettingsFilePath());

            string directoriPath = Path.Combine(LocalPath, LauncherSettings.COMPANY_NAME);
            if (!Directory.Exists(directoriPath))
                Directory.CreateDirectory(directoriPath);
            
            File.WriteAllText(path, settings.GameDirectory);
        }
        public static UserSettings LoadUserSettings()
        {
            UserSettings userSettings = new UserSettings();

            string path = Path.Combine(LocalPath, LauncherSettings.FullSettingsFilePath());

            if (File.Exists(path))
            {
                userSettings.GameDirectory = File.ReadAllText(path);
            }
            
            return userSettings; 
        }
    }
}
