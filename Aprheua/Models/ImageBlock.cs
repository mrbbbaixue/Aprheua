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
                RaisePropertyChanged("BlockPath");
            }
        }
        public Commands.DelegateCommand RemoveBlockClickEvent { get; set; }
        public ImageBlock(Commands.DelegateCommand removeBlockClickEvent)
        {
            RemoveBlockClickEvent = removeBlockClickEvent;
        }
    }
}
