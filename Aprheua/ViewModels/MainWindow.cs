using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.ViewModels
{
    public class MainWindow : NotificationObject
    {
        public string WindowTitle { get; set; }
        public ObservableCollection<Models.SourceImage> SourceImages { get; set; }
        public MainWindow()
        {
            WindowTitle = $"Aprheua - 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SourceImages = new ObservableCollection<Models.SourceImage> { };
        }
    }
}
