using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace ImageStore
{
    public partial class DataBaseSettingsWindow
    {
        public DataBaseSettingsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DatabaseOnline.Content = new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).Database.ToString();
            ServerOnline.Content = new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).DataSource.ToString();
        }

        public class ImageStoreSettingsEditing : ILinqToDBSettings
        {
            public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

            public string DefaultConfiguration => "ImageStore";
            public string DefaultDataProvider => "System.Data.SqlClient";

            public string DataBase;

            public string Server;

            public void DBAndServer (DataBaseSettingsWindow dataBaseSettingsWindow)
            {
                DataBase = dataBaseSettingsWindow.NameDB.Text;
                Server = dataBaseSettingsWindow.ServerName.Text;
            }

            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new Authorization.ConnectionSettings.ConnectionStringSettings
                        {
                            Name = "ImageStore",
                            ProviderName = "System.Data.SqlClient",
                            ConnectionString =
                            @"Data Source=" + Server.ToString() + ";" +
                            "Database =" + DataBase.ToString() + ";" +
                            "User ID=u0718435_Maxim;" +
                            "Password=Q1w2e3r4t5"
                        };
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var a = new ImageStoreSettingsEditing();
            a.DBAndServer(this);
            DataConnection.DefaultSettings = a;

            try
            {
                using (var db = new ImageStoreDB("ImageStore"))
                {
                    db.Connection.State.ToString();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.ConnectionStrings.ConnectionStrings["ImageStore"].ConnectionString = @"Data Source=" + ServerName.Text + ";Initial Catalog=" + NameDB.Text + ";";
                    config.Save();
                    Hide();
                    new DatabaseWindow().ShowDialog();
                    new LoginWindow().Show();
                }

            }
            catch (Exception)
            {
                new LoginWindow().Show();
                new ConnectWindow().ShowDialog();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Hide();
        private void Exit_Click(object sender, RoutedEventArgs e) => new CloseWindow().Show();

    }
}

