using System;
using System.Windows;
using System.Windows.Input;
using HandyControl.Interactivity;

namespace Aprheua.Commands
{
    public class DelegateCommand : ICommand
    {
          public bool CanExecute(object parameter)
          {
              if (CanExecuteFunc == null)
              {
                  return true;
              }

              return CanExecuteFunc(parameter);
          }
          public event EventHandler CanExecuteChanged;
          public void Execute(object parameter)
          {
              if (ExecuteAction == null)
              {
                  return;
              }
              ExecuteAction(parameter);
          }
          public Action<object> ExecuteAction { get; set; }
          public Func<object, bool> CanExecuteFunc { get; set; }

        public DelegateCommand(Action<object> action)
        {
            ExecuteAction = action;
        }
    }

    public class EventCommand : TriggerAction<DependencyObject>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty,value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventCommand), new PropertyMetadata(null));

        protected override void Invoke(object parameter)
        {
            if (CommandParameter != null)
            {
                parameter = CommandParameter;
            }
            var cmd = Command;
            cmd?.Execute(parameter);
        }
    }
}
