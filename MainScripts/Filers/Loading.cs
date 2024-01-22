using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TestLauncher
{
    public class Loading
    {
        public delegate void OnLoadComplete();
        public OnLoadComplete LoadComplete;

        public delegate void OnLoadNewStatus(GameStatus status);
        public OnLoadNewStatus NewStatusOfLoading;

        public delegate void WhenDeterminingTheDurationOfWork(long byteLength);
        public WhenDeterminingTheDurationOfWork WorkLengthNotify;

        public delegate void OnWorkProgress(long addBytes);
        public OnWorkProgress HttpDownloadProgress;
        public OnWorkProgress UnzipProgress;

        private CancellationTokenSource cancellationTokenSource = new();

        private byte[] buffer = new byte[1024*1024];//1Mb

        public async void Start()
        {
            //first stage
            NewStatusOfLoading?.Invoke(GameStatus.Loading);

            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = new();
            }

            using (HttpClient httpClient = new())
            {
                try
                {
                    await TryLoading(httpClient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Download error: " + ex.Message);
                    return;
                }
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                ClearLoadingFragments();
                return;
            }

            //next stage
            NewStatusOfLoading?.Invoke(GameStatus.Unpacking);

            try
            {
                await TryUnzip();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unzip error: " + ex.Message);
                return;
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                ClearLoadingFragments();
                return;
            }

            //completed successfully 
            NewStatusOfLoading?.Invoke(GameStatus.Ready);

            LoadComplete?.Invoke();
        }
        private async Task TryLoading(HttpClient httpClient)
        {
            Uri uri = new Uri(LauncherConst.LOAD_REFERENCE);

            using (Stream httpStream = await httpClient.GetStreamAsync(uri, cancellationTokenSource.Token))
            {
                using (WebClient wc = new())
                {
                    wc.OpenRead(uri);
                    long.TryParse(wc.ResponseHeaders["Content-Length"], out long length);

                    if (File.Exists(LauncherConst.FullZipPath()))
                        if (length == new FileInfo(LauncherConst.FullZipPath()).Length)
                            return;//The file has already been loaded

                    WorkLengthNotify?.Invoke(length);
                }
                using (FileStream zipCreate = File.Create(LauncherConst.FullZipPath()))
                {
                    long count = 0;
                    do //loading zip
                    {
                        await Task.Delay(1);
                        if (cancellationTokenSource.IsCancellationRequested)
                            return;

                        count = httpStream.Read(buffer, 0, buffer.Length);
                        zipCreate.Write(buffer, 0, (int)count);
                        HttpDownloadProgress?.Invoke(count);
                    }
                    while (count > 0);
                }
            }
        }
        private async Task TryUnzip()
        {
            using (FileStream reader = File.OpenRead(LauncherConst.FullZipPath()))
            {
                ZipArchive zip = new ZipArchive(reader);
                long count = 0;

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    count += entry.Length;
                }
                WorkLengthNotify?.Invoke(count);
                count = 0;

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (EntryIsDirectory(entry))
                    {
                        Directory.CreateDirectory(PathTo(entry));
                        continue;
                    }

                    using (Stream entryStresm = entry.Open())
                    using (FileStream unzipStream = File.Create(PathTo(entry)))
                    {
                        do //unzip
                        {
                            await Task.Delay(1);
                            if (cancellationTokenSource.IsCancellationRequested)
                                return;

                            count = entryStresm.Read(buffer, 0, buffer.Length);
                            unzipStream.Write(buffer, 0, (int)count);
                            UnzipProgress?.Invoke(count);
                        }
                        while (count > 0);
                    }
                }
            }
            bool EntryIsDirectory(ZipArchiveEntry entry)
            {
                return entry.FullName[entry.FullName.Length-1] == '/';
            }
            string PathTo(ZipArchiveEntry entry)
            {
                return Path.Combine(LauncherConst.FullUnzipPath(), entry.FullName);
            }
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private void ClearLoadingFragments()
        {
            if(File.Exists(LauncherConst.FullZipPath()))
                File.Delete(LauncherConst.FullZipPath());
            if (Directory.Exists(LauncherConst.FullGamePath()))
                Directory.Delete(LauncherConst.FullGamePath(), recursive: true);
        }
    }
}
