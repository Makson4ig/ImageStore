using System.Windows;
using System.Windows.Input;
using ImageStore.Authorization;

namespace ImageStore
{ 
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void ExitRegistration_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow().Show();
        }

        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void RegLogin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Registration().AuthorizationRegistration(TextBoxFIO.Text, TextBoxEmail.Text, TextBoxPost.Text, TextBoxLogin.Text, TextBoxPassword.Password,TextBoxReEnterPassword.Password);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow().Show();
        }
    }
}
