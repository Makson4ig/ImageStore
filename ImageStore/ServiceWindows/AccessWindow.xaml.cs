using System.Windows;
using ImageStore.PostOfficeWindows;

namespace ImageStore.ServiceWindows
{
    public partial class AccessWindow : Window
    {
        public AccessWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var sendingMessageWindow = new SendingMessageWindow();
            sendingMessageWindow.TextBoxLoginMail.Text = LoginNewUser.Text;
            sendingMessageWindow.TextBlockLoginNewUser.Text = LoginNewUser.Text;
            sendingMessageWindow.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow();
        }
    }
}
