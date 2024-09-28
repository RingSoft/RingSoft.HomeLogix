using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public interface IGetNewDateView
    {
        void Close();
    }
    public class GetNewDateViewModel : INotifyPropertyChanged
    {
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date == value)
                {
                    return;
                }
                _date = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OkCommand { get; }

        public RelayCommand CancelCommand { get; }

        public IGetNewDateView View { get; private set; }

        public ChangeDateData ChangeDateData { get; private set; }

        public GetNewDateViewModel()
        {
            OkCommand = new RelayCommand((() =>
            {
                OnOk();
            }));

            CancelCommand = new RelayCommand((() =>
            {
                ChangeDateData.DialogResult = false;
                View.Close();
            }));
        }

        public void Initialize(IGetNewDateView view, ChangeDateData changeData)
        {
            View = view;
            ChangeDateData = changeData;
            Date = changeData.NewDate;
        }

        private void OnOk()
        {
            var beginDate = new DateTime(Date.Year, Date.Month, 1);
            var endDate = beginDate.AddMonths(1).AddDays(-1);

            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<History>();

            var historyExists = table.Any(
                p => p.Date >= beginDate
                     && p.Date <= endDate
                     && p.ItemType != (byte)BudgetItemTypes.Transfer);

            var registerTable = context.GetTable<BankAccountRegisterItem>();

            var registerExists = registerTable.Any(
                p => p.ItemDate >= beginDate
                     && p.ItemDate <= endDate
                     && p.ItemType != (byte)BudgetItemTypes.Transfer);

            if (!registerExists && !historyExists)
            {
                var message = $"There is no data for the date specified";
                var caption = "Not a Valid Date";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return;
            }

            ChangeDateData.NewDate = Date;
            ChangeDateData.DialogResult = true;
            View.Close();

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
