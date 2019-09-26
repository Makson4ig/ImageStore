using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageStore.Authorization
{
    class RememberMe
    {
        public void InsertRemembers(LoginWindow loginWindow)
        {
            if (Properties.Settings.Default.Login != "")
            {
                loginWindow.TextBoxLogin.Text = Properties.Settings.Default.Login;
                loginWindow.TextBoxPassword.Password = Properties.Settings.Default.PasswordHash;
                loginWindow.CheckBoxIsRemember.IsChecked = true;
            }
        }

        public void AddRemembers(LoginWindow loginWindow)
        {
            if (loginWindow.CheckBoxIsRemember.IsChecked == false)
            {
                new RememberMe().SavePropertiesSettings(null, null);
            }
            if (loginWindow.CheckBoxIsRemember.IsChecked == true)
            {
                SavePropertiesSettings(loginWindow.TextBoxLogin.Text.ToString(), loginWindow.TextBoxPassword.Password);
            }
        }

        public void SavePropertiesSettings(string Login, string PasswordHash)
        {
            Properties.Settings.Default.Login = Login;
            Properties.Settings.Default.PasswordHash = PasswordHash;
            Properties.Settings.Default.Save();
        }

    }
}
