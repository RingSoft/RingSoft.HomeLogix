using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.App.Library
{
    public interface ISqliteLoginView
    { 
        string ShowFileDialog();
    }
    public class SqliteLoginViewModel : INotifyPropertyChanged
    {
        private string _fileNamePath;

        public string FilenamePath
        {
            get { return _fileNamePath; }
            set
            {
                if (_fileNamePath == value)
                {
                    return;
                }
                _fileNamePath = value;
                OnPropertyChanged();
            }
        }

        public ISqliteLoginView View { get; private set; }

        public RelayCommand ShowFileDialogCommand { get; }

        public string ModelName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SqliteLoginViewModel()
        {
            ShowFileDialogCommand = new RelayCommand(ShowFileDialog);
        }

        public void Initialize(ISqliteLoginView view)
        {
            View = view;
        }

        private void ShowFileDialog()
        {
            var fileName = View.ShowFileDialog();
            if (!fileName.IsNullOrEmpty())
                _fileNamePath = fileName;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
