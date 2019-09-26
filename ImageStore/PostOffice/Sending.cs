using ImageStore.Models.LinqToDB;
using LinqToDB;
using ImageStore.PostOfficeWindows;
using ImageStore.ServiceWindows;
using log4net;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Linq;
using System.Net;

namespace ImageStore.PostOffice
{
    class Sending
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Sending));
        public void SendCompany(SendingMessageWindow sendingMessageWindow)
        {
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);

                HttpWebRequest webRequest = WebRequest.Create("https://web2.elar.ru/ews/Exchange.asmx") as HttpWebRequest;
                webRequest.Proxy = WebRequest.DefaultWebProxy;
                webRequest.Proxy.Credentials = new NetworkCredential(sendingMessageWindow.TextBoxLoginMail.Text, sendingMessageWindow.TextBoxPasswordMail.Password, "techno");

                service.Url = new Uri("https://web2.elar.ru/ews/Exchange.asmx");
                service.Credentials = new WebCredentials(sendingMessageWindow.TextBoxLoginMail.Text, sendingMessageWindow.TextBoxPasswordMail.Password, "techno");

                AddRequestAccess(sendingMessageWindow);
                EmailMessage message = new EmailMessage(service)
                {
                    Subject = "Система ImageStore",
                    Body = new HtmlPage().pageHtml.ToString().
                    Replace("Access", sendingMessageWindow.ComboBoxLevelAccess.Text.Split('.')[1]).
                    Replace("Name", sendingMessageWindow.ComboBoxUserApprove.Text.Split('.')[1].Split('-')[0]).Replace("Login", sendingMessageWindow.TextBlockLoginNewUser.Text + ".")
                };
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
                message.ToRecipients.Add(SplitComboBoxUserApprove(sendingMessageWindow.ComboBoxUserApprove.Text));
                message.Save();
                message.SendAndSaveCopy();

                new MessageSentWindow().Show();
            }
            catch (ServiceResponseException ex)
            {
                var err = new ErrorWindow();
                err.TextErr.Text = ex.Message;
                err.Show();
            }
        }

        private string SplitComboBoxUserApprove(string text)
        {
            using (ImageStoreDB db = new ImageStoreDB("ImageStore"))
            {
                return db.Users.Where(x => x.UserId == int.Parse(text.Split('.')[0])).Select(x => x.Email).First().ToString();
            }
        }

        private void AddRequestAccess(SendingMessageWindow sendingMessageWindow)
        {
            using (ImageStoreDB db = new ImageStoreDB("ImageStore"))
            {
                var user = db.Users.Single(x => x.Login == sendingMessageWindow.TextBlockLoginNewUser.Text);
                user.RecipientAccessRequest = int.Parse(sendingMessageWindow.ComboBoxUserApprove.Text.Split('.')[0]);
                user.RequestAccess = sendingMessageWindow.ComboBoxLevelAccess.Text.Split('.')[1];
                db.Update(user);
            }
        }

        public void SendInternet()
        {
            //            try
            //            {
            //                MailMessage mail = new MailMessage();
            //                mail.From = new MailAddress(from);
            //                mail.To.Add(new MailAddress(mailto));
            //                mail.Subject = caption;
            //                mail.IsBodyHtml = true;
            //                mail.Body = @"";
            //                mail.IsBodyHtml = true;
            //                if (!string.IsNullOrEmpty(attachFile))
            //                    mail.Attachments.Add(new Attachment(attachFile));
            //                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //                client.EnableSsl = true;
            //                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
            //                client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //                client.Send(mail);
            //                mail.Dispose();
            //            }
            //            catch (Exception e)
            //            {
            //                Log.Error(e);
            //                throw new Exception("Mail.Send: " + e.Message);
            //            }
        }

    }
}
