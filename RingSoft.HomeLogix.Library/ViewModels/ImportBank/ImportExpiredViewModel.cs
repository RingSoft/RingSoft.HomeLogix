using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public interface IImportExpiredView
    {
        void CloseWindow();
    }
    public class ImportExpiredViewModel : INotifyPropertyChanged
    {
        private ImportExpiredGridManager _gridManager;

        public ImportExpiredGridManager GridManager
        {
            get => _gridManager;
            set
            {
                if (_gridManager == value)
                {
                    return;
                }
                _gridManager = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OkCommand { get; set; }

        public IImportExpiredView View { get; private set; }

        public ImportExpiredViewModel()
        {
            GridManager = new ImportExpiredGridManager(this);
            OkCommand = new RelayCommand(() =>
            {
                GridManager.UpdateList();
                View.CloseWindow();
            });
        }

        public void Initialize(IImportExpiredView view, List<BankAccountRegisterGridRow> rows)
        {
            View = view;
            GridManager.LoadGrid(rows);
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
