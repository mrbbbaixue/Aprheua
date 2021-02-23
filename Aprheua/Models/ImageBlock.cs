using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.Models
{
    public class ImageBlock : ViewModels.NotificationObject
    {
        private string _blockPath;
        public string BlockPath
        {
            get { return _blockPath; }
            set
            {
                _blockPath = value;
                this.RaisePropertyChanged("BlockPath");
            }
        }
    }
}
