using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.ViewModels
{
      public class NotificationObject : INotifyPropertyChanged
      {
          public event PropertyChangedEventHandler PropertyChanged;

          public void RaisePropertyChanged(string property)
          {
              if (this.PropertyChanged != null)
                  this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
          }
      }
}
