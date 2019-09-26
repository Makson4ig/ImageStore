using System;
using System.Linq;
using ImageStore.Authorization;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using LinqToDB;


namespace ImageStore.Drive
{
    public class Drive
    {
        public void ShowDrive(MainMenuWindow mainMenuWindow)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                mainMenuWindow.DataGridDrive.ItemsSource = db.Storages.Select(x => new
                {
                    x.StorageId,
                    TotalFreeSpace = (((x.TotalFreeSpace / 1024) / 1024) / 1024),
                    TotalSize = (((x.TotalSize / 1024) / 1024) / 1024),
                    x.SerialNumber,
                    x.DateOfLastUpdate,
                    x.DateAdded,
                    x.Description,
                    x.DateDeleted,
                    x.Name

                }).ToList();
            }
        }
        public void AddDrive(AddDriveWindow addDriveWindow)
        {
            using (ImageStoreDB db =new ImageStoreDB("ImageStore"))
            {
                var storage = new Storage();
                {
                    Int64 TotalFreeSpace = Convert.ToInt64(addDriveWindow.TextBoxTotalFreeSpace.Text);
                    Int64 TotalSize = Convert.ToInt64(addDriveWindow.TextBoxTotalSize.Text);
                    storage.StorageId = int.Parse(addDriveWindow.TextBoxStorageid.Text);
                    storage.ProjectId = 1;
                    storage.Name = addDriveWindow.TextBoxName.Text;
                    storage.TotalFreeSpace = TotalFreeSpace;
                    storage.TotalSize = TotalSize;
                    storage.SerialNumber = addDriveWindow.TextBoxSerialNumber.Text;
                    storage.DateOfLastUpdate = DateTime.Now;
                    storage.DateAdded = DateTime.Now;
                    storage.DateDeleted = null;
                    storage.StorageTypeId = SplitComboBox(addDriveWindow.ComboBoxStorageTypeid.Text);
                    storage.LocationId = SplitComboBox(addDriveWindow.ComboBoxLocationid.Text);
                    storage.OwnerUserId = SplitComboBox(addDriveWindow.ComboBoxUserId.Text);
                    storage.CurrentHolderUserId = new AuthorizationDataBase().GetLoginId(new MainMenuWindow().LabelLogin.Content.ToString());
                }
                db.Insert(storage);
                new AddedWindow().Show();
            }
        }
        public void DriveEmptyAndMore(int size,MainMenuWindow mainMenuWindow)
        {
            using (ImageStoreDB db = new ImageStoreDB("ImageStore"))
            {
                mainMenuWindow.DataGridDrive.ItemsSource = db.Storages.Where(x => (((x.TotalFreeSpace/1024)/1024)/1024) >= size).Select(x => new
                {
                    x.StorageId,
                    x.Name,
                    TotalFreeSpace = (((x.TotalFreeSpace / 1024) / 1024) / 1024),
                    TotalSize = (((x.TotalSize / 1024) / 1024) / 1024),
                    x.SerialNumber,
                    x.DateOfLastUpdate,
                    x.DateAdded,
                    x.Description,
                    x.DateDeleted

                }).ToList();
            }
        }
        private int  SplitComboBox(string text)
        {
            var strings = text.Split('.');
            return int.Parse(strings[0]);
        }
    }

}
