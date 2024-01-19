using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using ProgressBar = System.Windows.Controls.ProgressBar;

namespace TestLauncher
{
    public class ProgressBarView
    {
        private long progressLength = 0,
                     progressCurent = 0,
                     delta = 0,
                     lastDownloadMBytes = 0;
        private Timer timer = new() { Interval = 1000 };//1 sec

        private readonly SolidColorBrush greenProgressBar = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush coralProgressBar = new SolidColorBrush(Colors.Coral);
        private readonly string loading = "Загружено",
                                loadingSpeed = "загрузки",
                                unziping = "Распаковано",
                                unzipingSpeed = "распаковки";
        private bool isLoading;


        private TextBlock percentageText, loadingSpeedText;
        private ProgressBar progressBar;

        public ProgressBarView(TextBlock percentageText, TextBlock loadingSpeedText, ProgressBar progressBar)
        {
            this.percentageText = percentageText;
            this.loadingSpeedText = loadingSpeedText;
            this.progressBar = progressBar;
            timer.Tick += TimerTick;
        }

        public void WindowToStatus(GameStatus status)
        {
            bool isOnWork = status == GameStatus.Loading || status == GameStatus.Unpacking;
            isLoading = status == GameStatus.Loading;

            TimerWork(isOnWork);
            ProgressBarOnReady(status);
            ProgressBarColor(status);
            TextVisibility(isOnWork);
        }

        private void TimerWork(bool isOnWork)
        {
            if (timer.Enabled)
            {
                if (!isOnWork)
                    timer.Stop();
            }
            else
            {
                if (isOnWork)
                    timer.Start();
            }
        }
        private void ProgressBarOnReady(GameStatus status)
        {
            if (status == GameStatus.Ready)
            {
                progressBar.Value = progressBar.Maximum;
            }
        }
        private void ProgressBarColor(GameStatus status)
        {
            bool isHttpLoading = status == GameStatus.Loading || status == GameStatus.LoadRequired;
            progressBar.Foreground = isHttpLoading ? coralProgressBar : greenProgressBar;
        }
        private void TextVisibility(bool isOnWork)
        {
            Visibility visibility = isOnWork ? Visibility.Visible : Visibility.Hidden;
            percentageText.Visibility = visibility;
            loadingSpeedText.Visibility = visibility;
        }

        public void ProgressBarToMin()
        {
            if (progressBar == null)
                return;

            progressBar.Value = progressBar.Minimum;
        }
        public void DownloadProgress(long addBytes)
        {
            progressCurent += addBytes;
            SetProgressEvents();
        }
        public void WorkLength(long byteLength)
        {
            progressCurent = 0;
            progressLength = byteLength;
        }
        public void SetProgressEvents()
        {
            int ProgressPercentage = (int)(progressCurent / (progressLength/100));//%

            string keyWord = isLoading ? loading : unziping;
            percentageText.Text = $"{keyWord}: {ProgressPercentage}%";
            progressBar.Value = ProgressPercentage;

            delta += progressCurent - lastDownloadMBytes;
            lastDownloadMBytes = progressCurent;
        }
        public void TimerTick(object? sender, EventArgs e)
        {
            if (delta < 0)
                delta = 0;

            string speed = ToMB(delta).ToString("#.#");
            delta = 0;

            string keyWord = isLoading ? loadingSpeed : unzipingSpeed;
            loadingSpeedText.Text = $"Скорость {keyWord}: {speed}Мб/с";

            float ToMB(long bytes)
            {
                return (float)(bytes / 1048576f);//1024*1024 b=>Kb=>Mb
            }
        }
    }
}
