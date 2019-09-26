using System;
using System.Linq;
using System.Windows;
using ImageStore.LinqToDB.DataClassLinqToDB;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using log4net;

namespace ImageStore
{
    public partial class UsersWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UsersWindow));
        DataClassesImageStoreDataContext dataClasses = new DataClassesImageStoreDataContext(new ImageStoreDB("ImageStore").ConnectionString);
        public UsersWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ShowUsers();
        }

        public void ShowUsers()
        {
            if (dataClasses.DatabaseExists()) DataGridUsers.ItemsSource = dataClasses.User;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataClasses.SubmitChanges();
                new SaveWindow().ShowDialog();
                ShowUsers();
            }
            catch (Exception ex)
            {
                var err = new ErrorWindow();
                err.TextErr.Text = ex.Message;
                err.Show();
                Log.Error(ex);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
