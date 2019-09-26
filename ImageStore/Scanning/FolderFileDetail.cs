using ImageStore.Models.LinqToDB;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace ImageStore.Scanning
{
    public class FolderFileDetail
    {
        private int _id = 0;
        private bool _background = false;
        public void Traversal(string rootPath, ConcurrentQueue<Folder> collection)
        {
            Stack<(string path, int? parentId)> StatckPath = new Stack<(string path, int? parentId)>();
            StatckPath.Push((rootPath, null));

            var log = LogManager.GetLogger(typeof(FolderFileDetail));

            while (StatckPath.Any() && _background == false)
            {
                var curPath = StatckPath.Pop();
                var curFolder = GetFolder(curPath.path, curPath.parentId);
                collection.Enqueue(curFolder);
                log.Debug($@"Добавлена папка: {curFolder.FolderPath}/{curFolder.Name}");
                try
                {
                    foreach (var dir in Directory.GetDirectories(curPath.path))
                    {
                        StatckPath.Push((dir, curFolder.FolderId));
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    log.Warn(e);
                }
            }
            log.Debug($"Обход закончен: {DateTime.Now}");
        }

        private Folder GetFolder(string folderPath, int? parentId)
        {
            var dirInfo = new DirectoryInfo(folderPath);
            int totalSubFolderCount = 0;
            bool isError = false;
            try
            {
                totalSubFolderCount = dirInfo.GetDirectories().Count();
            }
            catch (UnauthorizedAccessException)
            {
                isError = true; /*Console.WriteLine(e);*/
            }

            var folder = new Folder()
            {
                FolderId = _id++,
                DateAdded = DateTime.Now,
                FolderPath = dirInfo.Parent?.FullName ?? dirInfo.FullName,
                Name = dirInfo.Parent != null ? dirInfo.Name : "*ROOT*",
                Lvl = folderPath.Count(x => x == '\\'),
                TotalSubFolderCount = totalSubFolderCount,
                ParentFolderId = parentId
            };

            folder.FileDetails = isError ? null : GetFileDetail(dirInfo, folder);
            return folder;
        }

        private List<FileDetail> GetFileDetail(DirectoryInfo dirInfo, Folder folder)
        {
            var fileDetailList = new List<FileDetail>();
            try
            {
                var result = dirInfo.GetFiles().GroupBy(x => x.Extension, StringComparer.InvariantCultureIgnoreCase)
                    .Select(group => new { Name = group.Key, Count = group.Count(), Sum = group.Sum(x => x.Length) })
                    .ToDictionary(key => key.Name, value => new { value.Count, value.Sum }, StringComparer.InvariantCultureIgnoreCase);
                foreach (var ext in result)
                {
                    fileDetailList.Add(new FileDetail()
                    {
                        ExtensionName = ext.Key,
                        FileCount = ext.Value.Count,
                        FileSize = ext.Value.Sum,
                        Folder = folder
                    });
                }
            }
            catch (UnauthorizedAccessException) { /*Console.WriteLine(e);*/ }
            return fileDetailList;
        }

        public void StoppingDataCollection()
        {
            this._background = true;
        }
    }
}
