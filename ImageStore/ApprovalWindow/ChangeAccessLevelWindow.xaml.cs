using ImageStore.Models.LinqToDB;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ImageStore.ApprovalWindow
{

    public partial class ChangeAccessLevelWindow : Window
    {
        public ChangeAccessLevelWindow(MainMenuWindow mainMenuWindow)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            VerificationRequest(mainMenuWindow);
        }


        public void VerificationRequest(MainMenuWindow mainMenuWindow)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                if (db.Users.Single(x => x.Login == mainMenuWindow.LabelLogin.Content.ToString()).PermissionsId == 1)
                {
                    NewUser(mainMenuWindow);
                }
            }
        }
        
        private void NewUser(MainMenuWindow mainMenuWindow)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                var nameCoordinator = db.Users.Single(x => x.Login == mainMenuWindow.LabelLogin.Content.ToString());

                if (db.Users.Where(x => x.RecipientAccessRequest == nameCoordinator.UserId).Count() > 0)
                {
                    List<int> NewUser = db.Users.Where(x => x.RecipientAccessRequest == nameCoordinator.UserId && x.RecipientAccessRequest != null && x.RequestAccess != null).Select(x => x.UserId).ToList();

                    foreach (var e in NewUser)
                    {
                        var User = db.Users.Single(x => x.UserId == e);

                        if (User.RequestAccess.ToString() != "")
                        {
                            TextBlockNewUser.Text = @"Пользватель " + User.Login + " запросил доступ '" + User.RequestAccess.ToString().Replace(" ","")+"'";
                            labelAccess.Content = User.RequestAccess.ToString();
                            labelLogin.Content = User.Login.ToString();
                            ShowDialog();
                        }
                    }
                }
                return;
            }
        }

        private void ExitMail_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Allow_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                var updatePermission = db.Users.Single(x => x.Login == labelLogin.Content.ToString());
                updatePermission.PermissionsId = Permission(labelAccess.Content.ToString().Replace(" ",""));
                updatePermission.RecipientAccessRequest = null;
                updatePermission.RequestAccess = null;
                db.Update(updatePermission);
            }
            Hide();
        }

        private int Permission(string Acess)
        {
            switch (Acess)
            {
                case "Администратор":
                    return 1;
                case "Модератор":
                    return 2;
                case "Пользователь":
                    return 3;
            }
            return 0;
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ImageStoreDB("ImageStore"))
            {
                var updatePermission = db.Users.Single(x => x.Login == labelLogin.Content.ToString());
                updatePermission.PermissionsId = 4;
                updatePermission.RecipientAccessRequest = null;
                updatePermission.RequestAccess = "Отказано";
                db.Update(updatePermission);
            }
            Hide();
        }
    }
}
