using log4net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using ImageStore.CopyingWindows;

namespace ImageStore.Copying
{
    class Copying
    {
        private CancellationToken _token;
        private int CountExistingFiles;
        private int CountError = -1;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Copying));
        public void CopyingMain(string FilePathes, CopyingWindow copyingWindow, CancellationToken token)
        {
            WriteToError(DateTime.Now + $"=================Запуск копирования=================");

            try
            {
                var dirs = File.ReadAllLines(FilePathes, Encoding.GetEncoding(Properties.Settings.Default.EncodeCode));
                ActCopying(copyingWindow, "Start of action", FilePathes, token);

                foreach (var dir in dirs)
                {
                    if (_token.IsCancellationRequested)
                    {
                        ActCopying(copyingWindow, "Action interrupt", FilePathes, token);
                        return;
                    }

                    ProgressBar(copyingWindow);
                    try
                    {
                        var dirArr = dir.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        Log.Debug("Копирование " + dirArr[0] + " --> " + dirArr[1]);
                        if (Path.GetExtension(dirArr[0]) != "" || Path.GetExtension(dirArr[1]) != "")
                        {
                            CopyFile(dirArr[0], dirArr[1]);
                        }
                        else
                        {
                            DirectoryCopy(dirArr[0], dirArr[1], true);
                        }
                        if (_token.IsCancellationRequested)
                        {
                            ActCopying(copyingWindow, "Action interrupt", FilePathes, token);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                }
                ActCopying(copyingWindow, "Action ending", FilePathes, token);
            }
            catch(Exception ex)
            {
                ActCopying(copyingWindow, "Action Error", FilePathes, token);
                Log.Error(ex);
            }
            
            
        }
        public void SendMessage(string msg, CopyingWindow copyingWindow)
        {
            copyingWindow.Dispatcher.BeginInvoke(new Action(delegate {copyingWindow.ListBoxLog.Items.Add(msg); }));
        }

        private void CopyFile(string srcFilePath, string dstFilePath)
        {
            if (Path.GetExtension(srcFilePath) == "" || Path.GetExtension(dstFilePath) == "")
            {
                Log.Error($"Один из путей не имеет расширения : {srcFilePath} -> {dstFilePath}");
                WriteToError(DateTime.Now + $" Error: Один из путей не имеет расширения : {srcFilePath} -> {dstFilePath}");
                throw new IOException($"Один из путей не имеет расширения : {srcFilePath} -> {dstFilePath}");
            }
            else
            {
                if (!File.Exists(dstFilePath))
                {
                    var targetDir = Path.GetDirectoryName(dstFilePath);
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    File.Copy(srcFilePath, dstFilePath, false);
                }
                else
                {
                    CountExistingFiles += 1;
                    Log.Error(" Файл уже существует: " + dstFilePath);
                    WriteToError(DateTime.Now + " Файл уже существует: " + dstFilePath);
                }
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Log.Error("Исходный каталог не существует или не может быть найден: " + sourceDirName);
                WriteToError(DateTime.Now + "Error: Исходный каталог не существует или не может быть найден: " + sourceDirName);
                throw new DirectoryNotFoundException("Исходный каталог не существует или не может быть найден: "+ sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (_token.IsCancellationRequested) return;
                string temppath = Path.Combine(destDirName, file.Name);
                if (!File.Exists(temppath))
                {
                    try
                    {
                        file.CopyTo(temppath, false);
                        Log.Debug("Копирую " + temppath);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                        WriteToError(ex.ToString());
                    }
                }
                else
                {
                    WriteToError(DateTime.Now + " Файл уже существует: " + temppath);
                    Log.Error("Файл уже существует: " + temppath);
                    CountExistingFiles += 1;
                }
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (_token.IsCancellationRequested) return;
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void ProgressBar(CopyingWindow copyingWindow)
        {
            copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.ProgressBarCopying.Value += 1; }));
        }

        private void ActCopying(CopyingWindow copyingWindow, string act, string filePathes, CancellationToken token)
        {
            switch (act)
            {
                case "Action interrupt":
                    if (CountExistingFiles != 0) { SendMessage(DateTime.Now + " ERROR Количество существующих файлов: " + CountExistingFiles, copyingWindow); }
                    if (CountError != 0)
                    {
                        SendMessage(DateTime.Now + " ERROR Присутствуют ошибки... Количество: " + CountError, copyingWindow);
                        SendMessage(DateTime.Now + " В папке с программой находится файл с ошибками FileError.txt ", copyingWindow);
                    }
                    SendMessage(DateTime.Now + " Копирование отменено...", copyingWindow);
                    copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.Copying.IsEnabled = true; }));
                    break;
                case "Action ending":
                    if (CountExistingFiles != 0) { SendMessage(DateTime.Now + " ERROR Количество существующих файлов: " + CountExistingFiles, copyingWindow); }
                    if (CountError != 0)
                    {
                        SendMessage(DateTime.Now + " ERROR Присутствуют ошибки... Количество: " + CountError, copyingWindow);
                        SendMessage(DateTime.Now + " В папке с программой находится файл с ошибками FileError.txt ", copyingWindow);
                    }
                    SendMessage(DateTime.Now + " Копирование завершено...", copyingWindow);
                    copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.Copying.IsEnabled = true; }));
                    break;
                case "Action Error":
                    if (CountExistingFiles != 0) { SendMessage(DateTime.Now + " ERROR Количество существующих файлов: " + CountExistingFiles, copyingWindow); }
                    if (CountError != 0)
                    {
                        SendMessage(DateTime.Now + " ERROR Присутствуют ошибки... Количество: " + CountError, copyingWindow);
                        SendMessage(DateTime.Now + " В папке с программой находится файл с ошибками FileError.txt ", copyingWindow);
                    }
                    SendMessage(DateTime.Now + " Ошибка", copyingWindow);
                    copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.Copying.IsEnabled = true; }));
                    break;
                case "Start of action":
                    SendMessage(DateTime.Now + " Запуск копирования...", copyingWindow);
                    _token = token;
                    copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.ProgressBarCopying.Maximum = File.ReadAllLines(filePathes).Length; }));
                    break;
            }
        }

        private void WriteToError(string text)
        {
            CountError += 1;
            string file = Path.Combine(Directory.GetCurrentDirectory(), "FileError.txt");
            File.AppendAllText(file, text + "\r\n");
        }
    }
}