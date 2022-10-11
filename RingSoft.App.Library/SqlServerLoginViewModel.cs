using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.App.Library
{
    public interface ISqlServerView
    {
        string Password { get; set; }
    }

    public class SqlServerLoginViewModel : INotifyPropertyChanged
    {
        private string _server;

        public string Server
        {
            get => _server;
            set
            {
                if (_server == value)
                {
                    return;
                }
                _server = value;
                OnPropertyChanged();
            }
        }

        private string _database;

        public string Database
        {
            get => _database;
            set
            {
                if (_database == value)
                {
                    return;
                }
                _database = value;
                OnPropertyChanged();
            }
        }
        private SecurityTypes _securityTypes;

        public SecurityTypes SecurityType
        {
            get => _securityTypes;
            set
            {
                if (_securityTypes ==  value)
                {
                    return;
                }
                _securityTypes = value;
                SecurityEnabled = value == SecurityTypes.SqlLogin;
                OnPropertyChanged();
            }
        }

        private bool _securityEnabled;

        public bool SecurityEnabled
        {
            get => _securityEnabled;
            set
            {
                if (_securityEnabled == value)
                {
                    return;
                }
                _securityEnabled = value;
                OnPropertyChanged();
            }
        }


        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value)
                {
                    return;
                }
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;
                OnPropertyChanged();
            }
        }

        public List<string> DatabasesList { get; private set; }

        public RelayCommand TestCommand { get; set; }

        public ISqlServerView View { get; set; }

        public SqlServerLoginViewModel()
        {
            TestCommand = new RelayCommand(TestConnection);
        }

        public void OnViewLoaded(ISqlServerView view)
        {
            View = view;
        }

        public void DatabaseGotFocus()
        {
            DatabasesList = RingSoftAppGlobals.GetSqlServerDatabaseList(Server);
        }

        private void TestConnection()
        {
            if (TestDatabaseConnection())
            {
                var message = "Connection Succeeded!";
                var caption = "Test Connection";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
            }
            else
            {
                var message = "Connection Failed!";
                var caption = "Test Connection";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Error);
            }
        }

        public bool TestDatabaseConnection()
        {
            var processor = new SqlServerDataProcessor();
            processor.Server = Server;
            processor.Database = "master";
            processor.SecurityType = SecurityType;
            switch (SecurityType)
            {
                case SecurityTypes.WindowsAuthentication:
                    break;
                case SecurityTypes.SqlLogin:
                    processor.UserName = UserName;
                    processor.Password = View.Password;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var query = new SelectQuery("spt_monitor");
            var result = processor.GetData(query);

            return result.ResultCode == GetDataResultCodes.Success;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
