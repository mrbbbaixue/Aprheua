using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.ViewModels
{
    public class AnalyseWindow : NotificationObject 
    {
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

        public AnalyseWindow()
        {
            WindowTitle = "HAAR 自动分析";
        }
    }
}
