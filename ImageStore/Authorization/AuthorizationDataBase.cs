using System;
using System.Linq;
using System.Security.Cryptography;
using ImageStore.Models.LinqToDB;
using ImageStore.ServiceWindows;
using LinqToDB.Data;
using log4net;
using static ImageStore.Authorization.ConnectionSettings;

namespace ImageStore.Authorization
{
    class AuthorizationDataBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthorizationDataBase));
        public Tuple<int, int> Login(string login, string password)
        {
            try
            {
                using (var db = new ImageStoreDB("ImageStore"))
                {
                    var user = db.Users.SingleOrDefault(x => x.Login == login);
                    return VerifyHashedPassword(user.PasswordHash, password) == true ? Tuple.Create(user.UserId, user.PermissionsId) : Tuple.Create(-1, -1);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Tuple.Create(-1, -1);
            }
        }


        /// <summary>
        /// Если нужен доступ к внешниму серверу нужно раскоментить и указать нужный ip. Так же нужно будет настроить доступ в ImageStoreIternetSettings 
        /// </summary>
        public void DatabaseSelection()
        {
            //if (new Ping().Send(IPAddress.Parse("31.31.196.202")).Status == IPStatus.Success)
            //{
            //DataConnection.DefaultSettings = new ImageStoreIternetSettings();
            //}
            //else
            //{
                 DataConnection.DefaultSettings = new ImageStoreSettings();
            //}
        }


        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        public string CheckConnection()
        {
            try
            {
                using (var db = new ImageStoreDB("ImageStore"))
                {
                    db.Connection.State.ToString();
                    return "1";
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return "-1";
            }
        }

        public int GetLoginId(string login)
        {
            try
            {
                using (var db = new ImageStoreDB("ImageStore"))
                {
                    var user = db.Users.SingleOrDefault(x => x.Login == login);
                    if (user != null) { return user.UserId; }
                    else { return -1; }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                var err = new ErrorWindow();
                err.TextErr.Text = ex.Message;
                err.Show();
                return -1;
            }
        }

    }
}
