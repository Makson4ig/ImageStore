using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageStore.CopyingWindows
{
    /// <summary>
    /// Логика взаимодействия для CopyingWindow.xaml
    /// </summary>
    public partial class CopyingWindow : Window
    {
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public CopyingWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Copying_Click(object sender, RoutedEventArgs e)
        {
            Copying.IsEnabled = false;
            cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            ListBoxLog.Items.Clear();
            ProgressBarCopying.Value = 0;

            var copying = new Copying.Copying();
            var path = TextBoxPathCopying.Text;

            var task = Task.Factory.StartNew(() =>
            {
                
                copying.CopyingMain(path, this ,token);

            }, TaskCreationOptions.LongRunning); 
        }
        
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            //cancelTokenSource = new CancellationTokenSource();
            //CancellationToken token = cancelTokenSource.Token;
            //ListBoxLog.Items.Clear();
            //ProgressBarCheck.Value = 0;

            //var linkValidity = new LinkValidity();
            //var path = TextBoxPathCheck.Text;

            //var task = Task.Factory.StartNew(() =>
            //{
            //    linkValidity.CheckLink(path);

            //}, TaskCreationOptions.LongRunning);
        }

        private void CopyingCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }

        private void CheckCancel_Click(object sender, RoutedEventArgs e)
        {
            //cancelTokenSource.Cancel();
        }

    }
}
