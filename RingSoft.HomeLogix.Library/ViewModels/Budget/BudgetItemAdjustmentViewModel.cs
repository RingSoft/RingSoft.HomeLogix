using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemAdjustmentView
    {
        void OnOkButtonCloseWindow();
    }

    public class BudgetItemAdjustmentViewModel : INotifyPropertyChanged
    {
        private string _budgetItemDescription;

        public string BudgetItemDescription
        {
            get => _budgetItemDescription;
            set
            {
                if (_budgetItemDescription == value)
                    return;

                _budgetItemDescription = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value)
                    return;
                ;

                _date = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;
                
                _description = value;
                OnPropertyChanged();
            }
        }

        private decimal? _projectedAdjustment;

        public decimal? ProjectedAdjustment
        {
            get => _projectedAdjustment;
            set
            {
                if (_projectedAdjustment == value)
                    return;

                _projectedAdjustment = value;
                OnPropertyChanged();
            }
        }

        private decimal? _actualAdjustment;

        public decimal? ActualAdjustment
        {
            get => _actualAdjustment;
            set
            {
                if (_actualAdjustment == value)
                    return;
                
                _actualAdjustment = value;
                OnPropertyChanged();
            }
        }


        public IBudgetItemAdjustmentView View { get; set; }

        public RelayCommand OkButtonCommand { get; }

        public bool DialogResult { get; set; }

        private BudgetItem _budgetItem;

        public BudgetItemAdjustmentViewModel()
        {
            OkButtonCommand = new RelayCommand(OnOkButton);
        }

        public void OnViewLoaded(IBudgetItemAdjustmentView view, BudgetItem budgetItem)
        {
            View = view;
            _budgetItem = budgetItem;
            BudgetItemDescription = budgetItem.Description;
            Date = budgetItem.LastCompletedDate.GetValueOrDefault(DateTime.Today);
        }

        private void OnOkButton()
        {
            var monthEndDate = new DateTime(Date.Year, Date.Month,
                DateTime.DaysInMonth(Date.Year, Date.Month));

            if (_budgetItem.CurrentMonthEnding == monthEndDate)
                _budgetItem.CurrentMonthAmount += ActualAdjustment.GetValueOrDefault(0);

            var budgetMonthHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(_budgetItem.Id,
                PeriodHistoryTypes.Monthly, monthEndDate) ?? new BudgetPeriodHistory
            {
                BudgetItemId = _budgetItem.Id,
                PeriodType = (byte)PeriodHistoryTypes.Monthly,
                PeriodEndingDate = monthEndDate
            };

            budgetMonthHistory.ProjectedAmount += ProjectedAdjustment.GetValueOrDefault(0);
            budgetMonthHistory.ActualAmount += ActualAdjustment.GetValueOrDefault(0);

            var yearEndDate = new DateTime(Date.Year, 12, 31);

            var budgetYearHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(_budgetItem.Id,
                PeriodHistoryTypes.Yearly, yearEndDate) ?? new BudgetPeriodHistory
            {
                BudgetItemId = _budgetItem.Id,
                PeriodType = (byte)PeriodHistoryTypes.Yearly,
                PeriodEndingDate = yearEndDate
            };

            budgetYearHistory.ProjectedAmount += ProjectedAdjustment.GetValueOrDefault(0);
            budgetYearHistory.ActualAmount += ActualAdjustment.GetValueOrDefault(0);

            var historyItem = new History
            {
                BankAccountId = _budgetItem.BankAccountId,
                Date = Date,
                ItemType = (int)_budgetItem.Type,
                BudgetItemId = _budgetItem.Id,
                Description = Description,
                ProjectedAmount = ProjectedAdjustment.GetValueOrDefault(0),
                ActualAmount = ActualAdjustment.GetValueOrDefault(0)
            };

            var periodHistoryRecords = new List<BudgetPeriodHistory>();
            periodHistoryRecords.Add(budgetMonthHistory);
            periodHistoryRecords.Add(budgetYearHistory);

            if (!AppGlobals.DataRepository.SaveBudgetItem(_budgetItem, periodHistoryRecords, historyItem))
                return;

            DialogResult = true;

            View.OnOkButtonCloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
