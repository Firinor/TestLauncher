using System.IO;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace TestLauncher
{
    public class LauncherSettings
    {
        public const string COMPANY_NAME = "FirGames";
        public const string SETTINGS_FILE_NAME = "Settings.save";
        public const string FOLDER_NAME = "Novel";
        public const string GAME_FILE_EXE = "Novel.exe";
        public const string GAME_FILE_ZIP = "Novel.zip";

        public static string FullExePath(UserSettings userSettings)
        {
            string path = userSettings.GameDirectory;
            return Path.Combine(path, COMPANY_NAME, FOLDER_NAME, GAME_FILE_EXE);
        }
        public static string FullZipPath(UserSettings userSettings)
        {
            string path = userSettings.GameDirectory;
            return Path.Combine(path, COMPANY_NAME, GAME_FILE_ZIP);
        }
        public static string FullSettingsFilePath()
        {
            return Path.Combine(COMPANY_NAME, SETTINGS_FILE_NAME);
        }
    }
}
