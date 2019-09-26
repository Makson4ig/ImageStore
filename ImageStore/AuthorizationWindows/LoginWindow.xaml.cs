using System.Windows;
using System.Windows.Input;
using ImageStore.Authorization;
using ImageStore.ServiceWindows;

namespace ImageStore
{
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            new AuthorizationDataBase().DatabaseSelection();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            new RememberMe().InsertRemembers(this);
        }
        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Window_Loaded() => TextBoxLogin.Focus();
        private void ExitLogin_Click(object sender, RoutedEventArgs e) => new CloseWindow().Show();
        private void Settings_Click(object sender, RoutedEventArgs e) => new DataBaseSettingsWindow().ShowDialog();
        private void RegLogin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RegistrationWindow().Show();
        }
       
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new RememberMe().AddRemembers(this);
            Hide();
            new Login().AuthorizationLogin(TextBoxLogin.Text, TextBoxPassword.Password);
        }
    }
}

