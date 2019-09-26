using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using log4net;
using ImageStore.UserWindows;

namespace ImageStore.Drive
{
    internal class DriveSearch
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DriveSearch));
        public void Search(string textBoxFolderPath, string textBoxStorageId, MainMenuWindow mainMenuWindow)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var db = new ImageStoreDB("ImageStore"))
                    {
                        if (textBoxFolderPath != "" && textBoxStorageId == "")
                        {
                            var Result = db.FindByPath(textBoxFolderPath).Take(Properties.Settings.Default.TopSelect).ToList();
                            mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate { mainMenuWindow.DataGridSearch.ItemsSource = Result.ToList();} ));
                        }

                        if (textBoxFolderPath == "" && textBoxStorageId != "")
                        {
                            var Result = db.FindByDisk(Convert.ToInt32(textBoxStorageId)).Take(Properties.Settings.Default.TopSelect).ToList();
                            mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate { mainMenuWindow.DataGridSearch.ItemsSource = Result.ToList();} ));
                        }

                        if (textBoxFolderPath == "" && textBoxStorageId == "")
                        {
                            var Result = db.Find().Take(Properties.Settings.Default.TopSelect).ToList();
                            mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate { mainMenuWindow.DataGridSearch.ItemsSource = Result.ToList(); }));
                        }

                        if (textBoxFolderPath != "" && textBoxStorageId != "")
                        {
                            var Result = db.FindByPathAndStorageId(textBoxFolderPath, Convert.ToInt32(textBoxStorageId)).Take(Properties.Settings.Default.TopSelect).ToList();
                            mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate { mainMenuWindow.DataGridSearch.ItemsSource = Result.ToList(); }));
                        }
                    }
                    mainMenuWindow.Dispatcher.BeginInvoke(new Action(delegate { mainMenuWindow.ProgressBarSearch.IsIndeterminate = false; mainMenuWindow.BtnSearch.IsEnabled = true; }));                    
                }
                catch (Exception ex)
                {
                    var err = new ErrorWindow();
                    err.TextErr.Text = ex.Message;
                    err.Show();
                    Log.Error(ex);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
