using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageStore.Authorization;
using ImageStore.Models.LinqToDB;
using log4net;
using LinqToDB;
using LinqToDB.Data;

namespace ImageStore.Scanning
{
    class DirectoryScanning
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DirectoryScanning));

        private BackgroundWorker _backgroundWorker;
        public void Scan(InputDataScan dataScan)
        {
            _backgroundWorker = dataScan.BackgroundWorker;
            log4net.Config.XmlConfigurator.Configure();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var db = new ImageStoreDB("ImageStore"))
            {
                try
                {
                    var authorizationDataBase = new AuthorizationDataBase();
                    var folderScanHistory = CreateFolderScanHistory(db, authorizationDataBase.GetLoginId(dataScan.Login));
                    ScanFolders(db, folderScanHistory, db.Storages.Single(x => x.StorageId == int.Parse(dataScan.DiskNumber)), dataScan.DiskPath);

                    if (_backgroundWorker.CancellationPending == false)
                    {
                        folderScanHistory.DateEndScan = DateTime.Now;

                        StorageFreeSpaceAndScanTime(db.Storages.Single(x => x.StorageId == int.Parse(dataScan.DiskNumber)), db, dataScan.DiskPath);

                        db.Update(folderScanHistory);

                        DeleteOldFolders(db, folderScanHistory, 
                            db.Storages.Single(x => x.StorageId == int.Parse(dataScan.DiskNumber)));

                        dataScan.BackgroundWorker.ReportProgress(1, $"Сбор статистики...");
                        Statistics(folderScanHistory.FolderScanHistoryId, dataScan);

                        Log.Debug("Готово!");
                        dataScan.BackgroundWorker.ReportProgress(1, "Готово!");
                    }
                    else
                    {
                        DeleteScannedAfterCancellation(db, folderScanHistory);
                        dataScan.BackgroundWorker.ReportProgress(1, "Отсканированые данные удалены...");
                    }
                }
                catch (Exception e)
                {
                    new FolderFileDetail().StoppingDataCollection();
                    Log.Error($"Критическая ошибка! Данные будут удалены джобом. " + e);
                    dataScan.BackgroundWorker.ReportProgress(1, "Критическая ошибка! Данные будут удалены джобом.");
                }
            }
            stopwatch.Stop();
            Log.Debug($"Прошло: {stopwatch.Elapsed.TotalSeconds} секунд \n Готово!");
            dataScan.BackgroundWorker.ReportProgress(1, $"Прошло: {stopwatch.Elapsed.TotalSeconds} секунд Готово!");
        }
        static void DeleteOldFolders(ImageStoreDB db, FolderScanHistory currentFolderScanHistory, Storage storage)
        {
            if (db.Folders.Any(x => x.Storage == storage && x.DateDeleted == null))
            {
                db.Folders.Where(folder => folder.DateDeleted == null &&
                folder.StorageId == storage.StorageId && folder.FolderScanHistoryId != currentFolderScanHistory.FolderScanHistoryId)
                    .Set(folder => folder.DateDeleted, DateTime.Now).Update();
            }
        }
        static void DeleteScannedAfterCancellation(ImageStoreDB db, FolderScanHistory currentFolderScanHistory)
        {
            currentFolderScanHistory.DeleteScannedAfterCancellation = "Отмена сканирования.";
            db.Update(currentFolderScanHistory);
            db.FileDetails.Where(x => x.FolderScanHistoryId == currentFolderScanHistory.FolderScanHistoryId).Delete();
            db.Folders.Where(x => x.FolderScanHistoryId == currentFolderScanHistory.FolderScanHistoryId).Delete();

        }
        static FolderScanHistory CreateFolderScanHistory(ImageStoreDB context, int userId)
        {
            var folderScanHistory = new FolderScanHistory() { CreatorUserId = userId, DateStartScan = DateTime.Now };
            folderScanHistory.FolderScanHistoryId = Convert.ToInt32(context.InsertWithIdentity(folderScanHistory));
            return folderScanHistory;
        }
        public void ScanFolders(ImageStoreDB db, FolderScanHistory folderScanHistory, Storage storage, string diskPath)
        {
            _backgroundWorker.ReportProgress(1, $"Сканирование...");
            _backgroundWorker.ReportProgress(1, $"Ожидание поступления порции...");

            var queue = new ConcurrentQueue<Folder>();
            var folderFileDetail = new FolderFileDetail();
            var secondsCompleteOperation = new BulkCopyOptions();
            var folderList = new List<Folder>();
            var defaultBatchSize = Properties.Settings.Default.BatchSize;

            var task = Task.Factory.StartNew(() =>
            {
                folderFileDetail.Traversal(diskPath, queue);
                
            }, TaskCreationOptions.LongRunning);

            while ((_backgroundWorker.CancellationPending == false) && (!task.IsCompleted || queue.Any() || folderList.Any()))
            {
                if (queue.TryDequeue(out var folder))
                {
                    folder.StorageId = storage.StorageId;
                    folder.FolderScanHistoryId = folderScanHistory.FolderScanHistoryId;
                    folderList.Add(folder);
                }

                Log.Debug($"Текущее кол-во элементов в очереди: {folderList.Count}/{defaultBatchSize}");
                if (folderList.Count >= defaultBatchSize || (task.IsCompleted && !queue.Any() && folderList.Any()))
                {
                    _backgroundWorker.ReportProgress(1, $"Текущее кол-во элементов в очереди: {folderList.Count}/{defaultBatchSize}");
                    Log.Debug($"Начало вставки порции: {DateTime.Now}");
                    _backgroundWorker.ReportProgress(1, "Начало вставки порции");

                    db.BulkCopy(folderList);
                    folderList.Where(x => x.FileDetails != null).SelectMany(x => x.FileDetails).ToList().ForEach(x =>
                    {
                        x.FolderId = x.Folder.FolderId;
                        x.FolderScanHistoryId = x.Folder.FolderScanHistoryId;
                    });
                    db.BulkCopy(secondsCompleteOperation, folderList.Where(x => x.FileDetails != null).SelectMany(x => x.FileDetails).ToList());
                    folderList.Clear();

                    Log.Debug($"Конец вставки порции: {DateTime.Now}");
                    _backgroundWorker.ReportProgress(1, "Конец вставки порции");
                    Thread.Sleep(1000);
                }
            }
            if (_backgroundWorker.CancellationPending)
            {
                folderFileDetail.StoppingDataCollection();
                Log.Debug($"Отмена Сканирования: {DateTime.Now}");
                _backgroundWorker.ReportProgress(1, "Отмена Сканирования");
                return;
            }
            Log.Debug($"Вставка полностью закончена: {DateTime.Now}");
            _backgroundWorker.ReportProgress(1, "Вставка полностью закончена");
            task.Wait();
        }
        static void StorageFreeSpaceAndScanTime(Storage storage, ImageStoreDB db, string diskPath)
        {
            storage.TotalFreeSpace = DriveInfo.GetDrives().Single(x => x.Name.Equals(diskPath, StringComparison.OrdinalIgnoreCase)).TotalFreeSpace;
            storage.TotalSize = DriveInfo.GetDrives().Single(x => x.Name.Equals(diskPath, StringComparison.OrdinalIgnoreCase)).TotalSize;
            storage.DateOfLastUpdate = DateTime.Now;
            db.Update(storage);
        }
        static void Statistics(int folderScanHistoryId, InputDataScan dataScan)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                dataScan.BackgroundWorker.ReportProgress(3, $"Количество Файлов: " + db.FileDetails.Where(x => x.FolderScanHistoryId == folderScanHistoryId).Sum(x => x.FileCount).ToString());
                dataScan.BackgroundWorker.ReportProgress(3, $"Количество JPG: " + db.FileDetails.Where(x => x.FolderScanHistoryId == folderScanHistoryId && x.ExtensionName == ".jpg").Sum(x => x.FileCount).ToString());
                dataScan.BackgroundWorker.ReportProgress(3, $"Количество Папок: " + db.Folders.Where(x => x.FolderScanHistoryId == folderScanHistoryId && x.FolderId > 0).Count());
            }
        }
    }
}
