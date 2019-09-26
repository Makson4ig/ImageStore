using System.Windows;


namespace ImageStore.ServiceWindows
{
    /// <summary>
    /// Логика взаимодействия для CloseWindow.xaml
    /// </summary>
    public partial class CloseWindow : Window
    {
        public CloseWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
