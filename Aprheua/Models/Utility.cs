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
        private string _logFilePath;
        //ToDo : 初始化streamwriter
        public LogWriter(string logfilePath)
        {
            _logFilePath = logfilePath;

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
