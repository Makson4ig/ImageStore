using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using LinqToDB;
using System.Windows;
using System.Windows.Controls;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ImageStore.LinqToDB.DataClassLinqToDB;

namespace ImageStore
{
    public partial class SettingsWindow
    {

        public SettingsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DatabaseOnline.Content = new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).Database.ToString();
            ServerOnline.Content = new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).DataSource.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["ImageStore"].ConnectionString = @"Data Source=" + ServerName.Text + ";Database=" + NameDB.Text + ";Integrated Security=SSPI;";
            config.Save();
            Hide();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Hide();
        private void Exit_Click(object sender, RoutedEventArgs e) => new CloseWindow().Show();

    }
}
