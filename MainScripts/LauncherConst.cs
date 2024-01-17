﻿using Path = System.IO.Path;

namespace TestLauncher
{
    public class LauncherConst
    {
        public static string USER_GAME_DIRECTORY = @"C:\Games\";

        public const string COMPANY_NAME = "FirGames";
        public const string SETTINGS_FILE_NAME = "Settings.json";
        public const string GAME_NAME = "Novel";
        public const string GAME_FILE_EXE = "Novel.exe";
        public const string GAME_FILE_ZIP = "Novel.zip";

        public const string LOAD_REFERENCE_HEAVY = @"https://downloader.disk.yandex.ru/disk/b1a603e4f391668464585ac085aee8477da2e296d39ca5ae73395b8bb907a2ed/65a80bc3/PuiZeFbp6I712swgcqHxGR5LeZnfSqyKnkRIHFjq23MJIdljn6QWuew3K-6K4bRr1FQvJNLMi7hdOWH0th9Oqg%3D%3D?uid=0&filename=Novel.zip&disposition=attachment&hash=4vQ7hdVDqb/ey6PJHiqon33Ba7e7uBNh80en4/OitqoYxBpLjImKsVWDmVU5rHVUq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=510115236&fsize=641124866&hid=9f2bde4f627273a3721ca62ff96ee848&media_type=compressed&tknv=v2";
        public const string LOAD_REFERENCE = @"https://downloader.disk.yandex.ru/disk/27afa876a6242c100432a3d0a7c3725f0281a393afb08ab4988d73fd21723de9/65a80bf6/PuiZeFbp6I712swgcqHxGUN3ILQ7ODfA9nz34LGcdWoWcO4bRSRppZFOCw871jWV7xFbACKtEPTC6kdCZjsMgA%3D%3D?uid=0&filename=NovelLite.zip&disposition=attachment&hash=194NL%2B6sb0tFbmFZHw7y/eCXRjaTSnskB3J1WEQGOoS2l73IeUmXmj9oXLSBzaZRq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=510115236&fsize=149540986&hid=82fbe311efbffbcd2d5f711b2124c289&media_type=compressed&tknv=v2";

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
