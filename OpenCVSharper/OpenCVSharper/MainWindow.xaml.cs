using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace OpenCVSharper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.GlowWindow
    {
        readonly Mat matSrcImage = new Mat($"test3.jpg", ImreadModes.AnyColor);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //load Source Image
            srcImage.Source = OpenCVExtendedClasses.MatToBitmapImage(matSrcImage);
            //TestCanny
            dstImage.Source = OpenCVExtendedClasses.MatToBitmapImage(TestFAST(matSrcImage));
        }

        //Testing methods
        private Mat TestCanny(Mat src)
        {
            Mat dst = new Mat();
            Cv2.Canny(src, dst, 50, 200);
            return dst;
        }

        private Mat TestFAST(Mat src)
        {
            Mat dst = src;
            FastFeatureDetector fastFeatureDetector = OpenCvSharp.FastFeatureDetector.Create(100);
            KeyPoint[] keyPoints = fastFeatureDetector.Detect(dst);
            Cv2.DrawKeypoints(dst,keyPoints,dst);
            return dst;
        }
    }

    public static class OpenCVExtendedClasses
    {
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }
        public static BitmapImage MatToBitmapImage(Mat image)
        {
            Bitmap bitmap = MatToBitmap(image);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // With transparency.

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
