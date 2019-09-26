using System;
using System.Linq;
using ImageStore.ApprovalWindow;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using log4net;

namespace ImageStore.Authorization
{
    public class Login
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Login));
        public void AuthorizationLogin(string textBoxLogin, string textBoxPassword)
        {
            try
            {
                
                var authorization = new AuthorizationDataBase();
                var mainMenuWindow = new MainMenuWindow();

                if (authorization.CheckConnection() == "1") 
                {
                    if (authorization.Login(textBoxLogin, textBoxPassword).Item1 == -1 &&
                        authorization.Login(textBoxLogin, textBoxPassword).Item2 == -1)
                    {
                        new LoginWindow().Show();
                        new WrongLoginPassword().Show();
                    }
                    else
                    {
                        mainMenuWindow.LabelLogin.Content = textBoxLogin;
                        new ChangeAccessLevelWindow(mainMenuWindow);
                        var permissions = LoginWindow(authorization.Login(textBoxLogin, textBoxPassword).Item2, mainMenuWindow);
                        if (permissions == 4) return;
                        mainMenuWindow.Show();
                    }
                }
                else
                {
                   new ConnectWindow().Show();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        public int LoginWindow(int accessRights, MainMenuWindow mainMenuWindow)
        {
            switch (accessRights)
            {
                case 1:
                    return 1;
                case 2:
                    mainMenuWindow.Users.IsEnabled = false;
                    return 2;
                case 3:
                    mainMenuWindow.Scan.IsEnabled = false;
                    mainMenuWindow.Users.IsEnabled = false;
                    mainMenuWindow.AddDrive.IsEnabled = false;
                    mainMenuWindow.Location.IsEnabled = false;
                    return 3;
                case 4:
                    mainMenuWindow.Scan.IsEnabled = false;
                    mainMenuWindow.Users.IsEnabled = false;
                    mainMenuWindow.Disk.IsEnabled = false;
                    mainMenuWindow.Settings.IsEnabled = false;
                    mainMenuWindow.Location.IsEnabled = false;
                    if (FailureCheck(mainMenuWindow) == 0)
                    {
                        var accessWindow = new AccessWindow();
                        accessWindow.LoginNewUser.Text = mainMenuWindow.LabelLogin.Content.ToString();
                        accessWindow.Show();
                    }
                    else
                    {
                        var accessDeniedWindow = new AccessDeniedWindow();
                        accessDeniedWindow.LoginNewUser.Text = mainMenuWindow.LabelLogin.Content.ToString();
                        accessDeniedWindow.Show();
                    }
                    return 4;
            }
            return -1;
        }

        private int FailureCheck(MainMenuWindow mainMenuWindow)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                var access = db.Users.Single(x => x.Login == mainMenuWindow.LabelLogin.Content.ToString());
                if (access.RequestAccess == "Отказано")
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
