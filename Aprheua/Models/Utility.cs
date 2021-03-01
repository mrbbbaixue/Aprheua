using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Aprheua.Models
{
    public class LogWriter
    {
        private string LogFilePath { get; }
        private StreamWriter Log { get; set; }
        private string CreatLogTxtPath { get; }
        public LogWriter(string logFilePath, string creatLogTxtPath)
        {
            LogFilePath = logFilePath + @"\Aprheua.log";
            CreatLogTxtPath = creatLogTxtPath + @"\Aprheua.txt";
        }
        public LogWriter(string path)
        {
            LogFilePath = path + @"\Aprheua.log";
            CreatLogTxtPath = path + @"\Aprheua.txt";
        }

        public void WriteLog(string logContent, LogType logType)
        {
            if (!string.IsNullOrEmpty(LogFilePath) && !string.IsNullOrEmpty(logContent))
            {
                if (File.Exists(LogFilePath))
                {
                    Log = File.AppendText(LogFilePath);
                }
                else
                {
                    Log = File.CreateText(LogFilePath);
                }
                Log.WriteLine($"[{logType}][Time:{DateTime.Now} {Utility.GetTimeStamp()}] {logContent}");
                Log.Close();
            }
            else
            {
                throw new Exception("Please Check The LogPath Or inputMessage.");
            }
        }

        public void OpenLogInTxt()
        {
            File.WriteAllText(CreatLogTxtPath, string.Empty);
            var logContent = File.ReadAllLines(LogFilePath);
            StreamWriter logTxt;
            if (File.Exists(CreatLogTxtPath))
            {
                logTxt = File.AppendText(CreatLogTxtPath);
            }
            else
            {
                logTxt = File.CreateText(CreatLogTxtPath);
            }
            foreach (var item in logContent)
            {
                logTxt.WriteLine(item);
            }
            logTxt.Close();
            System.Diagnostics.Process.Start("notepad", CreatLogTxtPath);
        }
    }
    public enum LogType
    {
        Info = 0,
        Error = 1,
        OpenCV = 2
    }
    public static class Utility
    {
        public static string GetFileHash(string path)
        {
            var hash = SHA1.Create();
            var stream = new FileStream(path, FileMode.Open);
            byte[] hashByte = hash.ComputeHash(stream);
            stream.Close();
            return BitConverter.ToString(hashByte).Replace("-", "");
        }
        public static long GetTimeStamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds;
            return timeStamp;
        }
        public static void DeleteFolder(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            ClearFolderContent(directoryPath, path => false);
            Directory.Delete(directoryPath);
        }
        private static void ClearFolderContent(string directoryPath, Func<FileSystemInfo, bool> keep)
        {
            var directory = new DirectoryInfo(directoryPath);
            foreach (var info in directory.EnumerateFileSystemInfos())
            {
                if (info is DirectoryInfo directoryInfo)
                {
                    ClearFolderContent(info.FullName, keep);
                    if (!directoryInfo.EnumerateFileSystemInfos().Any())
                    {
                        // delete directory if it's empty
                        directoryInfo.Delete();
                    }
                }
                else if (info is FileInfo fileInfo)
                {
                    if (keep(info))
                    {
                        continue;
                    }

                    fileInfo.IsReadOnly = false;
                    fileInfo.Delete();
                }
                else
                {
                    throw new NotImplementedException($"Unexpected FileSystemInfo type {info.GetType()}");
                }
            }
        }
    }

    /// <summary>   
    /// 缩略图处理类   
    /// 1、生成缩略图片或按照比例改变图片的大小和画质   
    /// 2、将生成的缩略图放到指定的目录下
    /// </summary>   
    public class ThumbImage
    {
        public Image ResourceImage;
        private int ImageWidth;
        private int ImageHeight;
        public string ErrMessage;

        /// <summary>   
        /// 类的构造函数   
        /// </summary>   
        /// <param name="ImageFileName">图片文件的全路径名称</param>   
        public ThumbImage(string ImageFileName)
        {
            ResourceImage = Image.FromFile(ImageFileName);
            ErrMessage = "";
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        //ToDo : （低优先级）增加1个重载，保持比例缩放到指定大小
        /* 即：在保持比例不变的情况下把长边缩小到指定大小
         * 比如 GetReducedImage(int targetMaxPixel, string targetFilePath)
         * GetReducedImage(50,"xxx.jpg");
         * 假设这张图的大小是100*50px，那处理后应该是50*25px
         * 假设这张图的大小是100*150px，那处理后应该是33.3 * 50px
         * 具体参考下面这个方法，只需要把下面这个方法复制一份，修改缩放大小就行
         * 最后记得和这个方法一样把图输出到指定位置 string targetFilePath
         */
        public bool GetReducedImage(int targetMaxPixel, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                double Percent = Convert.ToDouble(targetMaxPixel) / Convert.ToDouble(ResourceImage.Width > ResourceImage.Height ? ResourceImage.Width : ResourceImage.Height);
                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Height * Percent);
                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }
        /// <summary>   
        /// 生成缩略图方法，返回缩略图的Image对象   
        /// </summary>   
        /// <param name="Percent">缩略图的宽度百分比 如：0.8</param>     
        /// <param name="targetFilePath">缩略图保存的全文件名</param>   
        /// <returns>成功返回true,否则返回false</returns>   
        public bool GetReducedImage(double Percent, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Height * Percent);
                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

    }
}
