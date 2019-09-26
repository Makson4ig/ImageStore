using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using LinqToDB;
using log4net;

namespace ImageStore.Authorization
{
    public class Registration
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Registration));
        public void AuthorizationRegistration(string textBoxFIO,string textBoxEmail, string textBoxPost, string textBoxLogin, string textBoxPassword, string reEnterPassword)
        {
            try
            {
                var authorization = new AuthorizationDataBase();
                if (authorization.CheckConnection() == "1")
                {
                    using (var db = new ImageStoreDB("ImageStore"))
                    {
                        if (textBoxFIO == "" || textBoxEmail == "" || textBoxPost == "" || textBoxLogin == "" || textBoxPassword == "" || reEnterPassword == "") { MessageBox.Show("Пустые поля!", "Ошибка!"); return; }
                        if (textBoxPassword != reEnterPassword) { MessageBox.Show("Пароли не совпадают", "Ошибка!"); return; }
                        if (db.Users.Count(x => x.Login == textBoxLogin) != 0) { MessageBox.Show("Логин уже существует", "Ошибка!"); return; }

                        var user = new User();
                        {
                            user.FIO = textBoxFIO;
                            user.Email = textBoxEmail;
                            user.Post = textBoxPost;
                            user.Login = textBoxLogin;
                            user.PasswordHash = HashPassword(textBoxPassword);
                            user.DateAdded = DateTime.Now;
                            user.PermissionsId = 4;
                        }
                        db.Insert(user);
                    }
                    new LoginWindow().Show();
                }
                else
                {
                    new ConnectWindow().Show();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                new ConnectWindow().Show();
            }

        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }
}