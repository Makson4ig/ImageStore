using System.Windows;


namespace ImageStore.ServiceWindows
{
    /// <summary>
    /// Логика взаимодействия для WrongLoginPassword.xaml
    /// </summary>
    public partial class WrongLoginPassword : Window
    {
        public WrongLoginPassword()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
