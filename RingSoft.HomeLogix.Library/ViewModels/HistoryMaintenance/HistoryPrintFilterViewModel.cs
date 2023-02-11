using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public interface IHistoryFilterView
    {
        void CloseWindow();

        void SetValFailFocus();
    }

    public class HistoryPrintFilterCallBack
    {
        public DateTime FilterDate { get; set; }

        internal void FirePrintOutput(HistoryPrintFilterViewModel viewModel)
        {
            PrintOutput?.Invoke(this, viewModel);
        }
        public event EventHandler<HistoryPrintFilterViewModel> PrintOutput;
    }
    public class HistoryPrintFilterViewModel : INotifyPropertyChanged
    {
        private DateTime? _beginningDate;

        public DateTime? BeginningDate
        {
            get => _beginningDate;
            set
            {
                if (_beginningDate == value)
                {
                    return;
                }
                _beginningDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endingDate;

        public DateTime? EndingDate
        {
            get => _endingDate;
            set
            {
                if (_endingDate == value)
                {
                    return;
                }
                _endingDate = value;
                OnPropertyChanged();
            }
        }

        public IHistoryFilterView View { get; private set; }

        public HistoryPrintFilterCallBack CallBack { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public HistoryPrintFilterViewModel()
        {
            OkCommand = new RelayCommand(() =>
            {
                if (Validate())
                {
                    CallBack.FirePrintOutput(this);
                }
            });

            CancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize(IHistoryFilterView view, HistoryPrintFilterCallBack callBack)
        {
            View = view;
            CallBack = callBack;
            BeginningDate = EndingDate = callBack.FilterDate;
        }

        private bool Validate()
        {
            if (BeginningDate.HasValue && EndingDate.HasValue)
            {
                if (BeginningDate.Value > EndingDate.Value)
                {
                    var message = "The beginning date cannot be greater than the ending date.";
                    var caption = "Validation Failure";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    View.SetValFailFocus();
                    return false;
                }
            }
            return true;
        }

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
