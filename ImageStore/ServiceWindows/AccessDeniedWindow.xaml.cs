using ImageStore.PostOfficeWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageStore.ServiceWindows
{
    /// <summary>
    /// Логика взаимодействия для AccessDeniedWindow.xaml
    /// </summary>
    public partial class AccessDeniedWindow : Window
    {
        public AccessDeniedWindow()
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
            Application.Current.Shutdown();
        }
    }
}
