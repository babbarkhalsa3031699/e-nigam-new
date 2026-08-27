using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PVSWebSite.Utility
{
    public static class FileHelper
    {
        /// <summary>
        /// Copies a directory, its files and subdirectories to another directory.
        /// </summary>
        /// <param name="source">Fully qualified path for the source.</param>
        /// <param name="target">Fully qualified path for the target.</param>
        public static void CopyDirectory(string source, string target)
        {
            CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target));
        }

        /// <summary>
        /// Copies a directory, its files and subdirectories to another directory.
        /// </summary>
        /// <param name="source"><see cref="System.IO.DirectoryInfo"/> for the source.</param>
        /// <param name="target"><see cref="System.IO.DirectoryInfo"/> for the target.</param>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            target.CreateDirectoryIfNotExist();
            foreach (FileInfo fi in source.GetFiles())
            {
                CopyFile(fi, new FileInfo(Path.Combine(target.ToString(), fi.Name)));
            }


            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
            source = target = null;
        }

        public static void CopyFile(string source, string target)
        {
            CopyFile(new FileInfo(source), new FileInfo(target));
        }

        public static void CopyFile(FileInfo source, FileInfo target)
        {
            try
            {
                SetFileAccessToNormal(source.FullName);
                SetFileAccessToNormal(target.FullName);
                ReleaseFile(target);
                source.CopyTo(target.FullName, true);
                source = target = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recursively deletes all sub directories and main directory
        /// </summary>
        /// <param name="targetDir"></param>
        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                ReleaseFile(file);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            SetDirectoryAccessToNormal(targetDir);
            Directory.Delete(targetDir, false);
        }

        /// <summary>
        /// Recursively deletes all sub directories and main directory
        /// </summary>
        /// <param name="targetDir"></param>
        public static void DeleteDirectory(DirectoryInfo targetDir)
        {
            DeleteDirectory(targetDir.FullName);
        }



        /// <summary>
        /// Sets the file access to <see cref="FileAttributes.Normal"/> to enabler read/write
        /// </summary>
        /// <param name="path">Fully qualified file path.</param>
        public static void SetFileAccessToNormal(string path)
        {
            if (File.Exists(path))
                new FileInfo(path) { Attributes = FileAttributes.Normal };
        }

        /// <summary>
        /// Sets the directory access to <see cref="FileAttributes.Normal"/> to enabler read/write
        /// </summary>
        /// <param name="path">Fully qualified file path.</param>
        public static void SetDirectoryAccessToNormal(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            info.CreateDirectoryIfNotExist();
            info.Attributes = FileAttributes.Normal;

            foreach (FileInfo file in info.GetFiles())
                SetFileAccessToNormal(file.FullName);

            foreach (DirectoryInfo directory in info.GetDirectories())
                SetDirectoryAccessToNormal(directory.FullName);
            info = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns>Returns the fully qualified path</returns>
        public static string CreateDirectoryIfNotExist(this DirectoryInfo directoryInfo)
        {
            if (!Directory.Exists(directoryInfo.FullName))
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }
            return directoryInfo.FullName;
        }

        /// <summary>
        /// Caution : Use only for the file which is to be modified/deleted. This will clear all the contents of the target file.
        /// </summary>
        /// <param name="path"></param>
        public static void ReleaseFile(string path)
        {
            ReleaseFile(new FileInfo(path));
        }

        /// <summary>
        /// Caution : Use only for the file which is to be modified/deleted. This will clear all the contents of the target file.
        /// </summary>
        /// <param name="path"></param>
        public static void ReleaseFile(FileInfo path)
        {
            try
            {
                using (path.Create())
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get mime type for any file.
        /// </summary>
        /// <param name="fileName">The filename. Can be fully qualified or not.</param>
        /// <returns>The mime type or content type.</returns>
        public static string GetMimeType(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey(fileExtension);
            if (rk != null && rk.GetValue("Content Type") != null)
                return rk.GetValue("Content Type").ToString();
            return null;
        }

        /// <summary>
        /// Get relative path
        /// </summary>
        /// <param name="fromPath">From path</param>
        /// <param name="toPath">To path</param>
        /// <returns>Return relative path.</returns>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return "/" + relativePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public static void ReplaceInFile(FileInfo file, string oldValue, string newValue)
        {
            SetFileAccessToNormal(file.FullName);
            StringBuilder replacer = new StringBuilder(File.ReadAllText(file.FullName));
            replacer = replacer.Replace(oldValue, newValue);
            File.WriteAllText(file.FullName, replacer.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public static void ReplaceInFile(string fileFullPath, string oldValue, string newValue)
        {
            ReplaceInFile(new FileInfo(fileFullPath), oldValue, newValue);
        }

    }
}