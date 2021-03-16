/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/Utility.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   Utility 工具类，有来自作者其他Private Repo中的代
                   码。

    Classes:       LogWriter
                   // 用于记录日志并输出到文件和控制台窗口。
                   Utility
                   // 包含数个静态方法，包括获得文件哈希，获得时间戳，
                      删除文件夹，清空文件夹内容等。
                   ThumbImage
                   // 生成图像的缩略图并将其输出至文件。

****************************************************************/

using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Aprheua.Models
{
    public class LogWriter
    {
        private string LogFilePath { get; }
        private StreamWriter LogStreamWriter { get; set; }
        public LogWriter(string logFilePath)
        {
            LogFilePath = System.IO.Path.Combine(logFilePath, $"{App.LogFilePrefix}{Utility.GetTimeStamp()}.log");
        }

        private void Write(string logContent, LogType logType)
        {
            if (!string.IsNullOrEmpty(LogFilePath) && !string.IsNullOrEmpty(logContent))
            {
                if (File.Exists(LogFilePath))
                {
                    LogStreamWriter = File.AppendText(LogFilePath);
                }
                else
                {
                    LogStreamWriter = File.CreateText(LogFilePath);
                }
                Console.WriteLine($"$ [{logType}] {logContent}");
                LogStreamWriter.WriteLine($"[{logType}][Time:{DateTime.Now} {Utility.GetTimeStamp()}] {logContent}");
                LogStreamWriter.Close();
            }
            else
            {
                throw new Exception("Please Check The LogPath Or inputMessage!");
            }
        }
        public void Info(string content)
        {
            Write(content, LogType.Info);
            return;
        }
        public void Error(string content)
        {
            Write(content, LogType.Error);
            return;
        }
        public void OpenCV(string content)
        {
            Write(content, LogType.OpenCV);
            return;
        }
        public void OpenInNotepad()
        {
            System.Diagnostics.Process.Start("notepad", LogFilePath);
        }
        public enum LogType
        {
            Info = 0,
            Error = 1,
            OpenCV = 2
        }
    }

    public class ReversalBGR
    {
        public static double[] GetImageReversalBGR(Mat src)
        {
            try
            {
                double[] BGR = new double[3];
                double imagePX = src.Rows * src.Cols;
                for (int i = 0; i < src.Cols; i++)
                {
                    for (int j = 0; j < src.Rows; j++)
                    {
                        BGR[0] += src.At<Vec3b>(i, j)[0];
                        BGR[1] += src.At<Vec3b>(i, j)[1];
                        BGR[2] += src.At<Vec3b>(i, j)[2];
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    BGR[i] /= imagePX;
                    BGR[i] = 255 - BGR[i];
                }
                return BGR;
            }
            catch
            {
                return new double[3];
            }
        }
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
            ClearFolderContent(directoryPath, _ => false);
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
                    throw new Exception($"Unexpected FileSystemInfo type {info.GetType()}");
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
