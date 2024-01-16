using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using TestLauncher.MainScripts;

namespace TestLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        UserSettings userSettings;
        GameStatus gameStatus;

        public LauncherWindow()
        {
            userSettings = SaveManager.LoadUserSettings();

            InitializeComponent();

            CheckGameStatus();
            WindowToStatus();
        }

        private void CheckGameStatus()
        {
            string path = LauncherSettings.FullExePath(userSettings);
            if (File.Exists(path))
            {
                gameStatus = GameStatus.Ready;
                return;
            }

            path = LauncherSettings.FullZipPath(userSettings);
            if (File.Exists(path))
            {
                gameStatus = GameStatus.Unpacking;
                return;
            }

            //if nothing
            PathText.Text = userSettings.GameDirectory;
            gameStatus = GameStatus.None;
        }
        private void WindowToStatus()
        {
            Path.IsEnabled = gameStatus == GameStatus.None;

            string mainButtonContent;
            switch (gameStatus)
            {
                case GameStatus.Ready:
                    mainButtonContent = "ОТКРЫТЬ";
                    break;
                case GameStatus.Loading:
                    mainButtonContent = "ОСТАНОВИТЬ";
                    break;
                case GameStatus.Unpacking:
                    mainButtonContent = "ПОЧТИ";
                    break;
                case GameStatus.None:
                    mainButtonContent = "ЗАГРУЗИТЬ";
                    break;
                default:
                    throw new Exception("Unknown game status!");
            }

            Main_button.Content = mainButtonContent;
        }

        private void Main_button_Click(object sender, RoutedEventArgs e)
        {
            switch (gameStatus)
            {
                case GameStatus.None:
                    //StartLoaging();
                    break;
                case GameStatus.Loading:
                    //PauseLoading();
                    break;
                case GameStatus.Unpacking:
                    return;//nothing
                case GameStatus.Ready:
                    //OpenFolder();
                    break;
                default:
                    throw new Exception("Unknown game status!");
            }
        }
        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Minimize_button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            DragMove();
        }
        private void SetFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new ();
            DialogResult result = folderDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            PathText.Text = folderDialog.SelectedPath;
            userSettings.GameDirectory = folderDialog.SelectedPath;
            SaveManager.Save(userSettings);
        }
    }
}
