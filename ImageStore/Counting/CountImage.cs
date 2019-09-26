using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace ImageStore.CountImages
{
    public class CountImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Count { get; set; }
        public double Size { get; set; }

        private static readonly ILog Log = LogManager.GetLogger(typeof(CountImage));

        public void GetData(string start, MainMenuWindow mainMenuWindow, string regex, int considerSize)
        {
            try
            {
                var result = new List<CountImage>();
                var lines = File.ReadAllLines(start, Encoding.UTF8);
                Id = 0;

                foreach (string dir in lines)
                {
                    Id++;
                    if (dir != "")
                    {
                        if (Directory.Exists(dir))
                        {
                            string[] dirs = Directory.GetFiles(dir, regex, SearchOption.AllDirectories);

                            string dirpath;
                            int dircount;
                            if (considerSize == 1)
                            {
                                double totalSize = dirs.Select(file => new FileInfo(file))
                                    .Aggregate<FileInfo, double>(0, (current, fi) => current + fi.Length);
                                dircount = dirs.Length;
                                dirpath = dir;
                                result.Add(new CountImage
                                {
                                    Id = Id,
                                    Path = dirpath,
                                    Count = dircount.ToString(),
                                    Size = Math.Round((totalSize / Math.Pow(1024, 2)), 1)
                                });
                            }
                            else
                            {
                                dircount = dirs.Length;
                                dirpath = dir;
                                result.Add(new CountImage
                                {
                                    Id = Id,
                                    Path = dirpath,
                                    Count = dircount.ToString(),
                                });
                            }

                            ProgressBarValue(mainMenuWindow);
                        }
                        else
                        {
                            result.Add(new CountImage
                            {
                                Id = Id,
                                Path = dir,
                                Count = "Exists",
                            });
                            ProgressBarValue(mainMenuWindow);
                        }

                    }
                    else { ProgressBarValue(mainMenuWindow); }
                    
                }
                mainMenuWindow.Dispatcher?.BeginInvoke(new Action(delegate
                {
                    mainMenuWindow.DataGridCountImage.ItemsSource = result;
                    mainMenuWindow.Check.IsEnabled = true;
                    mainMenuWindow.Clear.IsEnabled = true;
                    mainMenuWindow.ProgressBarCountImage.Value = 0;
                }));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        
        public void ProgressBarValue(MainMenuWindow mainMenuWindow)
        {
            mainMenuWindow.Dispatcher?.BeginInvoke(new Action(delegate
            {
                mainMenuWindow.ProgressBarCountImage.Value++;
            }));
        }
    }
}

