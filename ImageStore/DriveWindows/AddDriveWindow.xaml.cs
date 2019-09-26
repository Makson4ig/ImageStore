using System;
using System.Linq;
using System.Windows;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using log4net;

namespace ImageStore
{
    public partial class AddDriveWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AddDriveWindow));
        public AddDriveWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboBoxSelect();
        }

        private void OkDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxStorageid.Text != "" &&
                    TextBoxName.Text != "" &&
                    TextBoxTotalFreeSpace.Text != "" &&
                    TextBoxTotalSize.Text != "" &&
                    TextBoxSerialNumber.Text != "" &&
                    ComboBoxLocationid.Text != "" &&
                    ComboBoxStorageTypeid.Text != "")
                {
                    new Drive.Drive().AddDrive(this);
                    Hide();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void CancelDrive_Click(object sender, RoutedEventArgs e) => Hide();

        private void ComboBoxSelect()
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                ComboBoxLocationid.ItemsSource = db.Locactions.Select(x =>  x.LocationId+". "+ x.Name).ToList();
                ComboBoxStorageTypeid.ItemsSource = db.StorageTypes.Select(x => x.StorageTypeId+". " +x.Name).ToList();
                ComboBoxUserId.ItemsSource = db.Users.Select(x => x.UserId + ". " + x.Login).ToList();
            }
        }
    }
}
