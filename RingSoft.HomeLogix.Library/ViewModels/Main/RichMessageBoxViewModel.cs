using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public interface IRichMessageBoxView
    {
        void CloseWindow();
    }

    public class HyperlinkData
    {
        public string UrlLink { get; set; }

        public string UserText { get; set; }

        public string TextToReplace { get; set; }
    }
    public class RichMessageBoxViewModel : INotifyPropertyChanged
    {
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                {
                    return;
                }
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _caption;

        public string Caption
        {
            get => _caption;
            set
            {
                if (_caption  == value)
                {
                    return;
                }
                _caption = value;
                OnPropertyChanged();
            }
        }



        public RelayCommand  OkCommand { get; set; }

        public IRichMessageBoxView View { get; private set; }

        public RichMessageBoxViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
        }

        public void Initialize(IRichMessageBoxView view)
        {
            View = view;
        }

        private void OnOk()
        {
            View.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
