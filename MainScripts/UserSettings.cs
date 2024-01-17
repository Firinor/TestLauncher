using System;
using System.ComponentModel;
using TestLauncher.MainScripts;

namespace TestLauncher
{
    public class UserSettings
    {
        private string _gameDirectory = "C:\\Games\\";
        public string GameDirectory
        {
            get
            {
                return LauncherConst.USER_GAME_DIRECTORY;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == _gameDirectory)
                    return;

                _gameDirectory = value;
                LauncherConst.USER_GAME_DIRECTORY = value;
                SaveManager.Save(this);
            }
        }
    }
}
