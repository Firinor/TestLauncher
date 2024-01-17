using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLauncher
{
    public class Loading
    {
        public delegate void OnLoadComplete();
        public OnLoadComplete LoadComplete;

        private LoadingElements loadingElements;

        private CancellationTokenSource cancellationTokenSource = new();
        private WebClient currentWebClient;

        public void Start()
        {
            using(WebClient webClient = new())
            {
                currentWebClient = webClient;
                Task.Run(() =>
                {
                    Uri uri = new Uri(LauncherConst.LOAD_REFERENCE);

                    if (loadingElements != null)
                    {
                        webClient.OpenRead(uri);
                        SetProgressEvents(loadingElements, webClient);
                    }

                    webClient.DownloadFileCompleted += OnLoadngEnd;
                    webClient.DownloadFileAsync(uri, LauncherConst.FullZipPath());
                }, 
                cancellationTokenSource.Token);
            }
        }

        private void SetProgressEvents(LoadingElements loadingElements, WebClient webClient)
        {
            long delta = 0;
            long lastDownloadMBytes = 0;
            //string size = webClient.ResponseHeaders["Countent-Length"];
            //float allMBytesToDownload = ToMB(long.Parse(size));

            StartTimer();

            webClient.DownloadProgressChanged += (s, e) =>
            {
                loadingElements.PersentageText.Text = "Загружено: " + e.ProgressPercentage + "%";
                loadingElements.ProgressBar.Value = e.ProgressPercentage;

                delta += e.BytesReceived - lastDownloadMBytes;
                lastDownloadMBytes = e.BytesReceived;
            };

            void TimerTick(object? sender, EventArgs e)
            {
                string speed = ToMB(delta).ToString("#.#");
                delta = 0;

                loadingElements.LoadingSpeedText.Text = "Скорость загрузки: " + speed + "Мб/с";
            }
            float ToMB(long bytes)
            {
                return (float)(bytes / 1048576f);//1024*1024 b=>Kb=>Mb
            }
            void StartTimer()
            {
                System.Windows.Forms.Timer timer = new();
                timer.Interval = 1000; //1 sec
                timer.Tick += TimerTick;
                timer.Start();
            }
        }

        private void OnLoadngEnd(object? sender, AsyncCompletedEventArgs e)
        {
            LoadComplete?.Invoke();
        }
        
        public void Stop()
        {
            if(currentWebClient != null)
            {
                currentWebClient.CancelAsync();
                cancellationTokenSource.Cancel();
            }
        }

        public void SetLoadingElements(LoadingElements loadingElements)
        {
            this.loadingElements = loadingElements;
        }
    }
}
