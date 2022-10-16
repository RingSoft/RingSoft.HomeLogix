using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.App.Library
{
    public enum DbLoginProcesses
    {
        Add = 0,
        Edit = 1,
        Connect = 2
    }

    public interface IDbLoginView
    {
        void CloseWindow();

        void SetPlatform();

        bool DataCopied { get; set; }

        bool DoCopyProcedure();
    }

    public abstract class DbLoginViewModel<TEntity> : INotifyPropertyChanged where TEntity : new()
    {
        private DbPlatforms _dbPlatform;

        public DbPlatforms DbPlatform
        {
            get => _dbPlatform;
            set
            {
                if (_dbPlatform == value)
                {
                    return;
                }
                _dbPlatform = value;
                OnPropertyChanged();
                View.SetPlatform();
                SetDefaultDatabaseName();
            }
        }

        private string _entityName;

        public string EntityName
        {
            get => _entityName;
            set
            {
                if (_entityName == value)
                    return;

                _entityName = value;
                SetDefaultDatabaseName();
                OnPropertyChanged();
            }
        }

        public DbPlatforms OriginalDbPlatform { get; set; }

        public string DbName { get; set; }

        public DbLoginProcesses DbLoginProcess { get; private set; }

        public SqliteLoginViewModel SqliteLoginViewModel { get; private set; }

        public SqlServerLoginViewModel SqlServerLoginViewModel { get; private set; }

        public IDbLoginView View { get; private set; }

        public TEntity Object { get; private set; }

        public RelayCommand OkCommand { get; }

        public RelayCommand CancelCommand { get; }

        public bool DialogResult { get; private set; } = true;

        private bool _loading;
        private bool _newSqliteFile;
        private bool _newSqlServerDatabase;
        private bool _settingSqliteFileName;
        private bool _settingSqlServerDatabase;
        private LookupContext _lookupContext;
        private DbDataProcessor _destinationProcessor;

        public DbLoginViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);

        }

        public virtual void Initialize(IDbLoginView view, DbLoginProcesses loginProcess, SqliteLoginViewModel sqliteLoginViewModel,
            SqlServerLoginViewModel sqlServerLoginViewModel, TEntity entity)
        {
            if (sqliteLoginViewModel == null || sqlServerLoginViewModel == null)
            {
                return;
            }
            View = view;
            DbLoginProcess = loginProcess;
            SqliteLoginViewModel = sqliteLoginViewModel;
            SqliteLoginViewModel.FileNameChanged += (sender, args) =>
            {
                if (!_loading && !_settingSqliteFileName)
                {
                    _newSqliteFile = false;
                }
            };
            SqlServerLoginViewModel = sqlServerLoginViewModel;
            SqlServerLoginViewModel.DatabaseChanged += (sender, args) =>
            {
                if (!_loading && !_settingSqlServerDatabase)
                {
                    _newSqlServerDatabase = false;
                }

            };
            
            if (entity != null)
            {
                _loading = true;
                LoadFromEntity(entity);
                OriginalDbPlatform = DbPlatform;
                Object = entity;
                _loading = false;
            }
            View.SetPlatform();
        }

        public abstract void LoadFromEntity(TEntity entity);

        public void SetDefaultDatabaseName()
        {
            var continueProcess = true;
            switch (DbLoginProcess)
            {
                case DbLoginProcesses.Add:
                case DbLoginProcesses.Edit:
                    switch (DbPlatform)
                    {
                        case DbPlatforms.Sqlite:
                            if (SqliteLoginViewModel.FilenamePath.IsNullOrEmpty())
                            {
                                if (_loading)
                                {
                                    continueProcess = false;
                                }
                                else
                                {
                                    _newSqliteFile = true;
                                }
                            }
                            else
                            {
                                if (!_loading)
                                {
                                    if (!_newSqliteFile)
                                    {
                                        continueProcess = false;
                                    }
                                }
                            }
                            break;
                        case DbPlatforms.SqlServer:
                            if (SqlServerLoginViewModel.Database.IsNullOrEmpty())
                            {
                                if (_loading)
                                {
                                    continueProcess = false;
                                }
                                else
                                {
                                    _newSqlServerDatabase = true;
                                }
                            }
                            else
                            {
                                if (!_loading)
                                {
                                    if (!_newSqlServerDatabase)
                                    {
                                        continueProcess = false;
                                    }
                                }
                            }
                            break;
                        case DbPlatforms.MySql:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case DbLoginProcesses.Connect:
                    continueProcess = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!continueProcess)
                return;

            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    var folder = Environment.GetEnvironmentVariable("OneDriveConsumer");
                    if (folder.IsNullOrEmpty())
                        folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var fileName = $"{EntityName} {DbName}.sqlite";
                    _settingSqliteFileName = true;
                    SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
                    _settingSqliteFileName = false;
                    break;
                case DbPlatforms.SqlServer:
                    var databaseName = EntityName.Replace(" ", "");
                    _settingSqlServerDatabase = true;
                    SqlServerLoginViewModel.Database = $"{databaseName}{DbName}";
                    _settingSqlServerDatabase = false;
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract void ShowEntityNameFailure();

        protected abstract void SaveEntity(TEntity entity);

        protected abstract bool PreDataCopy(ref LookupContext context, ref DbDataProcessor destinationProcessor);

        private void OnOk()
        {
            switch (DbLoginProcess)
            {
                case DbLoginProcesses.Add:
                case DbLoginProcesses.Edit:
                    if (EntityName.IsNullOrEmpty())
                    {
                        ShowEntityNameFailure();
                        DialogResult = false;
                        return;
                    }

                    var entity = new TEntity();
                    SaveEntity(entity);
                    Object = entity;
                    break;
                case DbLoginProcesses.Connect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (DbLoginProcess == DbLoginProcesses.Edit && OriginalDbPlatform != DbPlatform)
            {
                var message =
                    $"Do you wish to copy your data from {OriginalDbPlatform.PlatformText()} to {DbPlatform.PlatformText()}?";
                var caption = "Copy Data?";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) ==
                    MessageBoxButtonsResult.Yes)
                {
                    View.DataCopied = true;

                    if (!View.DoCopyProcedure())
                    {
                        return;
                    }
                }
            }

            View.CloseWindow();
        }

        public bool CopyData(ITwoTierProcedure procedure)
        {
            procedure.SetWindowText($"Copying Data from {OriginalDbPlatform.PlatformText()} to {DbPlatform.PlatformText()}");
            procedure.UpdateTopTier($"Creating Destination {DbPlatform.PlatformText()} Database", 100, 0);
            if (!PreDataCopy(ref _lookupContext, ref _destinationProcessor))
            {
                DialogResult = false;
                return false;
            }
            
            if (!RingSoftAppGlobals.CopyData(_lookupContext, _destinationProcessor, procedure))
            {
                DialogResult = false;
                return false;
            }

            return true;
        }

        protected void LoadDbDataProcessor(DbDataProcessor processor)
        {
            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    if (processor is SqliteDataProcessor sqliteDataProcessor)
                    {
                        sqliteDataProcessor.FilePath = SqliteLoginViewModel.FilePath;
                        sqliteDataProcessor.FileName = SqliteLoginViewModel.FileName;
                    }
                    break;
                case DbPlatforms.SqlServer:
                    if (processor is SqlServerDataProcessor sqlServerDataProcessor)
                    {
                        sqlServerDataProcessor.Server = SqlServerLoginViewModel.Server;
                        sqlServerDataProcessor.Database = SqlServerLoginViewModel.Database;
                        sqlServerDataProcessor.SecurityType = SqlServerLoginViewModel.SecurityType;
                        sqlServerDataProcessor.UserName = SqlServerLoginViewModel.UserName;
                        sqlServerDataProcessor.Password = SqlServerLoginViewModel.Password;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCancel()
        {
            DialogResult =false;
            View.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
