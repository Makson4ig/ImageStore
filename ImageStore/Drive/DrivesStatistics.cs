using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using ImageStore.Models.LinqToDB;


namespace ImageStore.Drive
{
    class DriveStatistics
    {
        public void ShowStatistics(MainMenuWindow mainMenuWindow)
        {
            Task.Factory.StartNew(() =>
            {
                using (var db = new ImageStoreDB("ImageStore"))
                {
                    List<string> list = new List<string>
                    {
                        "Всего дисков: " + db.Storages.Select(x => x.StorageId).Count(),
                        "Чистых дисков: " + db.Storages.Where(x => (((x.TotalFreeSpace / 1024) / 1024) / 1024) >= 1830).Select(x => x.StorageId).Count(),
                        "Папок отсканировано: " + db.Folders.Select(x => x.TotalSubFolderCount).Sum(),
                        "Файлов отсканировано: " + db.FileDetails.Select(x => x.FileCount).Sum(),
                        "Последние сканирование: " + db.FolderScanHistories.Where(x => x.DateEndScan != null).Select(x => x.DateEndScan).Max()
                    };

                    mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        mainMenuWindow.ListBoxStatistics.Items.Clear();

                        foreach(string e in list)
                        {
                            mainMenuWindow.ListBoxStatistics.Items.Add(e);
                        }

                    }));
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
