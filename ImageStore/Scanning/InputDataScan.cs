using System.ComponentModel;


namespace ImageStore.Scanning
{
    public class InputDataScan
    {
        public string DiskNumber { get; set; }
        public string DiskPath { get; set; }
        public string Login { get; set; }
        public BackgroundWorker BackgroundWorker { get; set; }

        public InputDataScan(BackgroundWorker backgroundWorker, MainMenuWindow mainMenuWindow)
        {
            DiskNumber = mainMenuWindow.DiskNumber.Text;
            DiskPath = mainMenuWindow.DiskPath.Text;
            Login = mainMenuWindow.LabelLogin.Content.ToString();
            BackgroundWorker = backgroundWorker;
        }

    }
}
