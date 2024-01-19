using Path = System.IO.Path;

namespace TestLauncher
{
    public class LauncherConst
    {
        public static string USER_GAME_DIRECTORY = @"C:\Games\";
        public static string LOAD_REFERENCE = @"https://downloader.disk.yandex.ru/disk/7d9708cd4d91354f5c1106a31e8de6549c538f366f416bfc09aa485cfd873682/65aae6ce/PuiZeFbp6I712swgcqHxGS-0THL2_hhyJrqD3XmP7nQXZaHLB1F6vQJmbmMR0_t_4IVVcZyXoIcj9X4sh1sybA%3D%3D?uid=510115236&filename=NovelPrototype_v0.7.zip&disposition=attachment&hash=&limit=0&content_type=application%2Fzip&owner_uid=510115236&fsize=219953968&hid=f25d66714a17bf2bd6b782b91d7f304f&media_type=compressed&tknv=v2&etag=fad0f1104de449097385bfc170dcd720";

        public const string COMPANY_NAME = "FirGames";
        public const string SETTINGS_FILE_NAME = "LauncherSettings.json";
        public const string GAME_NAME = "NovelPrototype";
        public const string GAME_FILE_EXE = "Novel.exe";
        public const string GAME_FILE_ZIP = "NovelPrototype_v0.7.zip";

        public static string FullExePath()
        {
            return Path.Combine(USER_GAME_DIRECTORY, COMPANY_NAME, GAME_NAME, GAME_FILE_EXE);
        }
        public static string FullGamePath()
        {
            return Path.Combine(USER_GAME_DIRECTORY, COMPANY_NAME, GAME_NAME);
        }
        public static string FullZipPath()
        {
            return Path.Combine(USER_GAME_DIRECTORY, COMPANY_NAME, GAME_FILE_ZIP);
        }
        public static string FullUnzipPath()
        {
            return Path.Combine(USER_GAME_DIRECTORY, COMPANY_NAME);
        }
        public static string SettingsFilePath()
        {
            return Path.Combine(COMPANY_NAME, SETTINGS_FILE_NAME);
        }
    }
}
