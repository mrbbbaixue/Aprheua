/****************************************************************

    File Name:     ViewModels/DisposableImage.cs

    Author:        ybx (https://blog.csdn.net/u012559285)

    Version:       1.0.0.0

    Date:          2016-12-07

    Description:   Image控件的继承

    Classes:       DisposableImage : Image
                   // 为解决Image读取后无法释放的问题，重写的Disposa
                      bleImage

    License:       CC 4.0 BY-SA

    Source:        https://blog.csdn.net/u012559285/article/details/76887341

****************************************************************/

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Aprheua.ViewModels
{
    /// <summary>
    /// 为解决Image读取后无法释放的问题，重写的DisposableImage
    /// from : https://blog.csdn.net/u012559285/article/details/76887341
    /// </summary>
    public class DisposableImage : Image
    {
        public new string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisposableSource.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(DisposableImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(DisposableImage.OnSourceChanged), null), null);

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image image = (Image)d;
            if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()) || !File.Exists(e.NewValue.ToString()))
            {
                return;
            }

            using (BinaryReader reader = new BinaryReader(File.Open(e.NewValue.ToString(), FileMode.Open)))
            {
                    FileInfo fi = new FileInfo(e.NewValue.ToString());
                    byte[] bytes = reader.ReadBytes((int)fi.Length);
                    reader.Close();

                    BitmapImage bitmapImage = new BitmapImage
                    {
                        CacheOption = BitmapCacheOption.OnLoad
                    };

                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(bytes);
                    bitmapImage.EndInit();
                    image.Source = bitmapImage;
            }
        }
    }
}
