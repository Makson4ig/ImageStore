using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageStore.Drive;
using ImageStore.Models.LinqToDB;
using ImageStore.Scanning;
using ImageStore.ServiceWindows;
using log4net;
using Microsoft.Win32;
using System.Threading.Tasks;
using ImageStore.CopyingWindows;
using ImageStore.CountImages;
using ImageStore.UserWindows;


namespace ImageStore
    
{
    public partial class MainMenuWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainMenuWindow));
        public MainMenuWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _backgroundWorker = (BackgroundWorker)FindResource("BackgroundWorker");
        }

        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            SetupWindow("GridMain");
            new DriveStatistics().ShowStatistics(this);
        }
        
        /// <summary>
        /// Открытие разных окон
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e) { new SettingsWindow().ShowDialog(); }
        private void Users_Click(object sender, RoutedEventArgs e) => new UsersWindow().ShowDialog();
        private void Location_Click(object sender, RoutedEventArgs e) => new LocationSettingsWindow().ShowDialog();
        private void Copying_Click(object sender, RoutedEventArgs e) => new CopyingWindow().Show();

        /// <summary>
        /// Поиск массива
        /// </summary>
        public void Search_Click(object sender, RoutedEventArgs e) => SetupWindow("GridSearch");
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //BtnSearch.IsEnabled = false;
            ProgressBarSearch.IsIndeterminate = true;
            new DriveSearch().Search(TextBoxFolderPath.Text, TextBoxStorageId.Text, this);
        }

        /// <summary>
        /// Добавление и поиск дисков
        /// </summary>
        private void DriveEmpty_Checked(object sender, RoutedEventArgs e) => new Drive.Drive().DriveEmptyAndMore(1830, this);
        private void DriveSizeMore_Checked(object sender, RoutedEventArgs e) => new Drive.Drive().DriveEmptyAndMore(50, this);
        private void DriveEmptyAndMore_Unchecked(object sender, RoutedEventArgs e) => new Drive.Drive().ShowDrive(this);
        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            SetupWindow("GridDisk");
            new Drive.Drive().ShowDrive(this);
        }
        private void AddDrive_Click(object sender, RoutedEventArgs e)
        {
            new AddDriveWindow().Show();
            new Drive.Drive().ShowDrive(this);
        }

        /// <summary>
        /// Манипуляции с окном
        /// </summary>
        private void MaximizedWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Exit_Click(object sender, RoutedEventArgs e) => new CloseWindow().Show();

        /// <summary>
        /// Для подсчета графических образов
        /// </summary>
        private int _considerSize;
        private void CountImage_Click(object sender, RoutedEventArgs e) => SetupWindow("GridCountImage");
        private void Size_Checked(object sender, RoutedEventArgs e) => _considerSize = 1;
        private void Size_Unchecked(object sender, RoutedEventArgs e) => _considerSize = 0;
        private void Clear_Click(object sender, RoutedEventArgs e) => DataGridCountImage.ItemsSource = null;
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Regex.Text == "") Regex.Text = "*.*";

                ProgressBarCountImage.Maximum = File.ReadAllLines(PathFile.Text).Length;

                Check.IsEnabled = false;
                Clear.IsEnabled = false;
                var mainMenuWindow = this;
                var text = PathFile.Text;
                var regextext = Regex.Text;
                var size = _considerSize;
                
                Task.Factory.StartNew(() =>
                {
                    new CountImage().GetData(text, mainMenuWindow, regextext, size);

                }, TaskCreationOptions.LongRunning);
            }

            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Text documents (*.txt)|*.txt" };
            dialog.ShowDialog();
            PathFile.Text = dialog.FileName;
        }

        /// <summary>
        /// Сканирование дисков
        /// </summary>
        private readonly BackgroundWorker _backgroundWorker;
        private void Scan_Click(object sender, RoutedEventArgs e) => SetupWindow("GridScan");
        private void ScanDisk_Click(object sender, RoutedEventArgs e)
        {
            ListBoxLogScan.Items.Clear();
            ListBoxStat.Items.Clear();
            var inputDataScan = new InputDataScan(_backgroundWorker, this);

            if (DiskNumber.Text == "" || DiskPath.Text == "" || CheckPathAndDiskNumber(inputDataScan.DiskNumber, inputDataScan.DiskPath) != "Done") return;
          
            ScanDisk.IsEnabled = false;
            ScanDiskCancel.IsEnabled = true;
            ProgressBar.IsIndeterminate = true;
            _backgroundWorker.RunWorkerAsync(inputDataScan);
        }
        private void ScanDiskCancel_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.IsIndeterminate = false;
            _backgroundWorker.CancelAsync();
            new CancelWindow().Show();
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            var inputDataScan = (InputDataScan)e.Argument;
            var directoryScanning = new DirectoryScanning();
            directoryScanning.Scan(inputDataScan);
        }
        private void BackgroundWorker_OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                DeleteItemsListBox(ListBoxLogScan);
                ListBoxLogScan.Items.Add(DateTime.Now + " " + e.UserState);
            }
            if (e.ProgressPercentage == 3)
            {
                ListBoxStat.Items.Add(e.UserState.ToString());
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            ScanDisk.IsEnabled = true;
            ScanDiskCancel.IsEnabled = false;
            ProgressBar.IsIndeterminate = false;
        }
        private void DeleteItemsListBox(ListBox listBox)
        {
            if (listBox.Items.Count <= 7) return;
            for (var i = listBox.Items.Count - 15; i >= 0; i--)
            {
                listBox.Items.RemoveAt(i);
            }
        }
        private string CheckPathAndDiskNumber(string diskNumber, string diskPath)
        {
            try
            {
                if (((new ImageStoreDB("ImageStore").Storages.Any(x => x.StorageId == int.Parse(diskNumber)) == false) || new DirectoryInfo(diskPath).Exists == false))
                {
                    ListBoxLogScan.Items.Add("Пути не существует или Номер диска некорректный...");
                    return "Error";
                }
                return "Done";
            }
            catch
            {
                ListBoxLogScan.Items.Add("Пути не существует или Номер диска некорректный...");
                return "Error";
            }
        }

        /// <summary>
        /// Выбор какое окно будет открыто
        /// </summary>
        /// <param name="whatWindow"></param>
        public void SetupWindow(string whatWindow)
        {
            switch (whatWindow)
            {
                case "GridScan":
                    GridScan.Visibility = Visibility.Visible;
                    GridCountImage.Visibility = Visibility.Hidden;
                    GridMain.Visibility = Visibility.Hidden;
                    GridSearch.Visibility = Visibility.Hidden;
                    GridDrive.Visibility = Visibility.Hidden;
                    break;
                case "GridCountImage":
                    GridCountImage.Visibility = Visibility.Visible;
                    GridScan.Visibility = Visibility.Hidden;
                    GridMain.Visibility = Visibility.Hidden;
                    GridSearch.Visibility = Visibility.Hidden;
                    GridDrive.Visibility = Visibility.Hidden;
                    break;
                case "GridMain":
                    GridMain.Visibility = Visibility.Visible;
                    GridCountImage.Visibility = Visibility.Hidden;
                    GridScan.Visibility = Visibility.Hidden;
                    GridSearch.Visibility = Visibility.Hidden;
                    GridDrive.Visibility = Visibility.Hidden;
                    break;
                case "GridSearch":
                    GridSearch.Visibility = Visibility.Visible;
                    GridMain.Visibility = Visibility.Hidden;
                    GridCountImage.Visibility = Visibility.Hidden;
                    GridScan.Visibility = Visibility.Hidden;
                    GridDrive.Visibility = Visibility.Hidden;
                    break;
                case "GridDisk":
                    GridDrive.Visibility = Visibility.Visible;
                    GridSearch.Visibility = Visibility.Hidden;
                    GridMain.Visibility = Visibility.Hidden;
                    GridCountImage.Visibility = Visibility.Hidden;
                    GridScan.Visibility = Visibility.Hidden;
                    break;
            }
        }

    }
}

