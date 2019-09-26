using System.Windows;

namespace ImageStore.ServiceWindows
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class CancelWindow : Window
    {
        public CancelWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void OkCancelScan_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
