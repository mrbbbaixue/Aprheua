/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     ViewModels/NotificationObject.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   NotificationObject 类的实现

    Classes:       NotificationObject : INotifyPropertyChanged
                   // 主要为后台源提供 RaisePropertyChanged 方法，
                      用于通知Views属性变化

****************************************************************/

using System.ComponentModel;

namespace Aprheua.ViewModels
{
    public class NotificationObject : INotifyPropertyChanged
      {
          public event PropertyChangedEventHandler PropertyChanged;
          public void RaisePropertyChanged(string property)
          {
              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
      }
}
