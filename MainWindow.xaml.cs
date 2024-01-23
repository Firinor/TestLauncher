using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using TestLauncher.MainScripts;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TestLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        private UserSettings userSettings;
        private Loading loading = new();
        private ProgressBarView progressBarView;
        private bool subscribe = false;

        private GameStatus gameStatus;

        public LauncherWindow()
        {
            InitializeComponent();

            LoadLocalSettings();
            InitializeLauncherFields();
            CheckStatus();
        }

        private void LoadLocalSettings()
        {
            userSettings = SaveManager.LoadUserSettings();
            PathText.Text = userSettings.GameDirectory;
        }
        private void InitializeLauncherFields()
        {
            progressBarView = new ProgressBarView(PercentageText, LoadingSpeedText, LoadingProgressBar);
        }
        private void CheckStatus()
        {
            Subscribe();
            CheckGameStatus();
            WindowToStatus();
        }
        private void Subscribe()
        {
            if (subscribe)
                return;

            loading.NewStatusOfLoading += NewWindowStatus;
            loading.WorkLengthNotify += progressBarView.WorkLength;
            loading.HttpDownloadProgress += progressBarView.DownloadProgress;
            loading.UnzipProgress += progressBarView.DownloadProgress;
            subscribe = true;
        }
        private void CheckGameStatus()
        {
            string path = LauncherConst.FullExePath();
            if (File.Exists(path))
            {
                //CheckUpdate();
                gameStatus = GameStatus.Ready;
                return;
            }

            if (isZipReady())
            {
                gameStatus = GameStatus.UnpackRequired;
                return;
            }

            //if nothing
            gameStatus = GameStatus.LoadRequired;
        }

        private void NewWindowStatus(GameStatus newStatus)
        {
            if (gameStatus == newStatus)
                return;

            gameStatus = newStatus;
            WindowToStatus();
        }
        private void WindowToStatus()
        {
            PathText.IsEnabled = gameStatus == GameStatus.LoadRequired;
            progressBarView.WindowToStatus(gameStatus);

            string mainButtonContent = "";
            switch (gameStatus)
            {
                case GameStatus.Ready:
                    mainButtonContent = "ОТКРЫТЬ";
                    break;
                case GameStatus.LoadRequired:
                    mainButtonContent = "ЗАГРУЗИТЬ";
                    break;
                case GameStatus.UnpackRequired:
                    mainButtonContent = "РАСПАКОВАТЬ";
                    break;
                case GameStatus.Loading:
                    mainButtonContent = "ОСТАНОВИТЬ";
                    break;
                case GameStatus.Unpacking:
                    mainButtonContent = "ОСТАНОВИТЬ";
                    break;
                default:
                    System.Windows.MessageBox.Show("Unknown game status!");
                    //throw new Exception("Unknown game status!");
                    break;
            }

            Main_button.Content = mainButtonContent;
        }

        private void Main_button_Click(object sender, RoutedEventArgs e)
        {
            switch (gameStatus)
            {
                case GameStatus.LoadRequired:
                    CreateInstallDirectory();
                    loading.Start();
                    break;
                case GameStatus.Loading:
                    gameStatus = GameStatus.LoadRequired;
                    loading.Stop();
                    progressBarView.ProgressBarToMin();
                    break;
                case GameStatus.UnpackRequired:
                    gameStatus = GameStatus.Unpacking;
                    loading.Start();
                    break;
                case GameStatus.Unpacking:
                    gameStatus = GameStatus.LoadRequired;
                    loading.Stop();
                    progressBarView.ProgressBarToMin();
                    break;
                case GameStatus.Ready:
                    if (Directory.Exists(LauncherConst.FullUnzipPath()))
                    {
                        Process.Start("explorer.exe", LauncherConst.FullGamePath());
                    }
                    else
                    {
                        MessageBox.Show("The game directory could not be found!");
                        CheckGameStatus();
                    }
                    break;
                default:
                    MessageBox.Show("Unknown game status!");
                    //throw new Exception("Unknown game status!");
                    break;
            }
            WindowToStatus();
        }

        private void CreateInstallDirectory()
        {
            Directory.CreateDirectory(LauncherConst.FullUnzipPath());
        }
        private bool isZipReady()
        {
            return File.Exists(LauncherConst.FullZipPath());
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
            if (!PathText.IsEnabled)
            {
                if(Directory.Exists(PathText.Text))
                    Process.Start("explorer.exe", PathText.Text);

                return;
            }

            FolderBrowserDialog folderDialog = new ();
            DialogResult result = folderDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            PathText.Text = folderDialog.SelectedPath;
            userSettings.GameDirectory = folderDialog.SelectedPath;
        }
        private void PathText_LostFocus(object sender, RoutedEventArgs e)
        {
            userSettings.GameDirectory = PathText.Text;
        }
    }
}
