using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.App.Library
{
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
