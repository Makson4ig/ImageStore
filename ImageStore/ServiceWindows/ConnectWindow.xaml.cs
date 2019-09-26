using System.Windows;

namespace ImageStore.ServiceWindows
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }
        private void OkCancelScan_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Application.Current.Shutdown();
        }
    }
}
