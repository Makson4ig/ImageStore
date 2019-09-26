using System;
using System.Windows;
using System.Windows.Input;
using ImageStore.LinqToDB.DataClassLinqToDB;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using log4net;

namespace ImageStore.UserWindows
{
    /// <summary>
    /// Логика взаимодействия для LocationSettingsWindow.xaml
    /// </summary>
    public partial class LocationSettingsWindow 
    {
        DataClassesImageStoreDataContext dataClasses = new DataClassesImageStoreDataContext(new ImageStoreDB("ImageStore").ConnectionString);
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationSettingsWindow));
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        public LocationSettingsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ShowLocation();
        }

        public void ShowLocation()
        {
            if (dataClasses.DatabaseExists()) DataGridLocation.ItemsSource = dataClasses.Locaction;
            if (dataClasses.DatabaseExists()) DataGridLocationType.ItemsSource = dataClasses.LocationType;
        }
        private void SaveLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataClasses.SubmitChanges();
                new SaveWindow().ShowDialog();
                ShowLocation();
            }
            catch (Exception ex)
            {
                var err = new ErrorWindow();
                err.TextErr.Text = ex.Message;
                err.Show();
                Log.Error(ex);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e) => Hide();
    }
}
