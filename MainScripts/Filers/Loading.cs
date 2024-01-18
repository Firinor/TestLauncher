using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
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

        private byte[] buffer = new byte[1024*1024];//1Mb

        public async void Start()
        {
            Uri uri = new Uri(LauncherConst.LOAD_REFERENCE);
            try
            {
                cancellationTokenSource.TryReset();
                using (HttpClient httpClient = new())
                {
                    int count = 0;
                    using (Stream httpStream = await httpClient.GetStreamAsync(uri, cancellationTokenSource.Token))
                    {
                        do //loading zip
                        {
                            count = httpStream.Read(buffer, 0, buffer.Length);

                        }
                        while (count > 0);

                        using (Stream unzipStream = File.Create(LauncherConst.FullUnzipPath()))
                        using (GZipStream zip = new GZipStream(unzipStream, CompressionMode.Decompress))
                        {
                            do //unzip
                            {
                                count = httpStream.Read(buffer, 0, buffer.Length);

                                zip.Write(buffer, 0, count);
                            }
                            while (count > 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
            if (e.Cancelled)
            {
                ClearLoadingFragments();
                return;
            }

            if (e.Error != null)
            {
                MessageBox.Show("Loading error: " + e.Error.ToString());
                ClearLoadingFragments();
                return;
            }

            //completed successfully 
            LoadComplete?.Invoke();
        }

        private void ClearLoadingFragments()
        {
            if(File.Exists(LauncherConst.FullZipPath()))
                File.Delete(LauncherConst.FullZipPath());
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public void SetLoadingElements(LoadingElements loadingElements)
        {
            this.loadingElements = loadingElements;
        }
    }
}
