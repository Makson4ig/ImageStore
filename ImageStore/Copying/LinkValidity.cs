using log4net;
using System;
using ImageStore.CopyingWindows;

namespace ImageStore.Copying
{
    class LinkValidity
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Copying));
        public void CheckLink(string pathtxt, CopyingWindow copyingWindow)
        {
            //var count = 0;
            //var linkAllLines = File.ReadAllLines(pathtxt, Encoding.GetEncoding(Properties.Settings.Default.EncodeCode));

            //foreach (var link in linkAllLines)
            //{
            //    try
            //    {
            //        if (!File.Exists(link) || !Directory.Exists(link))
            //        {

            //        }
            //        else
            //        {
            //            count += 1;
            //        }

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }

        public void SendMessage(string msg, CopyingWindow copyingWindow)
        {
            copyingWindow.Dispatcher.BeginInvoke(new Action(delegate { copyingWindow.ListBoxLog.Items.Add(msg); }));
        }

    }
}
