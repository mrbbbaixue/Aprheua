using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprheua.Commands;

namespace Aprheua.ViewModels
{
    public class MainWindow : NotificationObject
    {
        //Datas
        private string _windowTitle;
        public string WindowTitle 
        { 
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                this.RaisePropertyChanged("WindowTitle");
            }
        }
        public ObservableCollection<Models.SourceImage> SourceImages { get; set; }

        //Commands
        public DelegateCommand ImportCommand { get; set; }
        public void Import(object parameter)
        {
            var sourceImage = new Models.SourceImage
            {
                Path = @"E:\GitHub\Hi-Icy\Aprheua\2.jpg",
                Name = $"Test2.jpg"
            };
            SourceImages.Add(sourceImage);
        }
        public MainWindow()
        {
            //Datas
            WindowTitle = $"Aprheua - 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SourceImages = new ObservableCollection<Models.SourceImage> { };
            //Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
        }
    }
}
