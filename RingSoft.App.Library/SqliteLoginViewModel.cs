using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.App.Library
{
    public interface ISqliteLoginView
    { 
        string ShowFileDialog();

        void SetFocusToControl();
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
                FileNameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string FileName
        {
            get
            {
                if (_fileNamePath.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                try
                {
                    var fileInfo = new FileInfo(FilenamePath);

                    return fileInfo.Name;
                }
                catch (Exception e)
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "File Error!", RsMessageBoxIcons.Error);
                    return string.Empty;
                }
                
            }
        }

        public string FilePath
        {
            get
            {
                if (_fileNamePath.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                try
                {
                    var fileInfo = new FileInfo(FilenamePath);
                    return fileInfo.DirectoryName;
                }
                catch (Exception e)
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "File Error!", RsMessageBoxIcons.Error);
                    return string.Empty;
                }

            }
        }



        public ISqliteLoginView View { get; private set; }

        public RelayCommand ShowFileDialogCommand { get; }

        public string ModelName { get; set; }

        public DbLoginProcesses LoginProcess { get; set; }

        public event EventHandler FileNameChanged;

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
                FilenamePath = fileName;
        }

        public bool TestConnection(string tableName)
        {
            switch (LoginProcess)
            {
                case DbLoginProcesses.Add:
                case DbLoginProcesses.Edit:
                    return true;
                
                case DbLoginProcesses.Connect:
                    var processor = new SqliteDataProcessor();
                    processor.FilePath = FilePath;
                    processor.FileName = FileName;
                    var result = processor.GetData(new SelectQuery(tableName) { MaxRecords = 1 }).ResultCode == GetDataResultCodes.Success;
                    if (!result)
                    {
                        var message = "Invalid filename.";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, message, RsMessageBoxIcons.Exclamation);
                        View.SetFocusToControl();
                    }
                    return result;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
