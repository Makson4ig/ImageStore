using ImageStore.Models.LinqToDB;
using ImageStore.PostOffice;
using ImageStore.ServiceWindows;
using log4net;
using System;
using System.Linq;
using System.Windows;

namespace ImageStore.PostOfficeWindows
{
    public partial class SendingMessageWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SendingMessageWindow));

        public SendingMessageWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboBoxSelect();
        }

        private void OkMail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxLoginMail.Text != "" &&
                    TextBoxPasswordMail.Password != "" &&
                    ComboBoxLevelAccess.Text != "" &&
                    ComboBoxUserApprove.Text != "")
                {
                    new Sending().SendCompany(this);
                    Hide();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                var err = new ErrorWindow();
                err.TextErr.Text = ex.Message;
                err.Show();
            }
        }

        private void ExitMail_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow().Show();
        }

        private void ComboBoxSelect()
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                ComboBoxLevelAccess.ItemsSource = db.Permissions.Where(x => x.PermissionsId != 4).Select(x => x.PermissionsId + ". " + x.PermissionsName).ToList();
                ComboBoxUserApprove.ItemsSource = db.Users.Where(x => x.PermissionsId == 1).Select(x => x.UserId + ". " + x.FIO + " - " + x.Post).ToList();
            }
        }
    }
}
