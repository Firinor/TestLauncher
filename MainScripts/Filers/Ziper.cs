using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLauncher
{
    public class Ziper
    {
        public delegate void OnUnzipComplete();
        public OnUnzipComplete UnzipComplete;

        private bool isCancel;

        public void Unzip()
        {
            if (File.Exists(LauncherConst.FullZipPath()))
            {
                TryUnzip();
            }
            else
                MessageBox.Show("The zip archive could not be found!");
        }

        private async void TryUnzip()
        {
            isCancel = false;

            if (!Directory.Exists(LauncherConst.FullGamePath()))
                Directory.CreateDirectory(LauncherConst.FullGamePath());

            await Task.Run(async () =>
            {
                using (ZipArchive archive = ZipFile.Open(LauncherConst.FullZipPath(), ZipArchiveMode.Read))
                {
                    //archive.ExtractToDirectory(LauncherConst.FullGamePath(), overwriteFiles: false);
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (isCancel)
                            break;

                        string entryPath = Path.Combine(LauncherConst.FullGamePath(), entry.FullName);

                        bool isFolder = entry.Name == "";
                        entryPath = entryPath.Replace("/", "\\");
                        if (isFolder)
                        {
                            if (!Directory.Exists(entryPath))
                                Directory.CreateDirectory(entryPath);
                        }
                        else
                        {
                            if (!File.Exists(entryPath))
                                try
                                {
                                    entry.ExtractToFile(entryPath, overwrite: false);
                                }
                                catch (Exception ex) 
                                { 
                                    //Вне тестового задания разобраться с кодировкой архивов
                                }
                        }

                        await Task.Yield();
                    }
                }
            });

            if (isCancel)
            {
                try
                {
                    File.Delete(LauncherConst.FullGamePath());
                }
                catch 
                { 
                    //Не удаляем файлы, к которым нет доступа                
                }
            }
            else //Success
            {
                //File.Delete(LauncherConst.FullZipPath());
                UnzipComplete?.Invoke();
            } 
        }

        public void Stop()
        {
            isCancel = true;
        }
    }
}
