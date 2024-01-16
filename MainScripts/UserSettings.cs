using System;
using System.ComponentModel;

namespace TestLauncher
{
    public class UserSettings
    {
        private string _gameDirectory = "C:\\Games\\";
        public string GameDirectory
        {
            get
            {
                return _gameDirectory;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value != _gameDirectory)
                    _gameDirectory = value;
            }
        }
    }
}
