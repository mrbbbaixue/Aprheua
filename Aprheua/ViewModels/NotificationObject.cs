using System.ComponentModel;

namespace Aprheua.ViewModels
{
    public class NotificationObject : INotifyPropertyChanged
      {
          public event PropertyChangedEventHandler PropertyChanged;
          public void RaisePropertyChanged(string property)
          {
              if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
      }
}
