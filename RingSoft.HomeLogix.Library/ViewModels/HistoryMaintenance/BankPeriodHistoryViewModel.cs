using System;
using System.ComponentModel;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BankPeriodHistoryViewModel : DbMaintenanceViewModel<BankAccountPeriodHistory>
    {

        private string _bankAccount;

        public string BankAccount
        {
            get => _bankAccount;
            set
            {
                if (_bankAccount == value)
                {
                    return;
                }

                _bankAccount = value;
                OnPropertyChanged();
            }
        }

        private DateTime _periodEndingDate;

        public DateTime PeriodEndingDate
        {
            get => _periodEndingDate;
            set
            {
                if (_periodEndingDate == value)
                {
                    return;
                }

                _periodEndingDate = value;
                OnPropertyChanged();
            }
        }

        private double _totalDeposits;

        public double TotalDeposits
        {
            get => _totalDeposits;
            set
            {
                if (_totalDeposits == value)
                {
                    return;
                }

                _totalDeposits = value;
                OnPropertyChanged();
            }
        }

        private double _totalWithdrawals;

        public double TotalWithdrawals
        {
            get => _totalWithdrawals;
            set
            {
                if (_totalWithdrawals == value)
                {
                    return;
                }

                _totalWithdrawals = value;
                OnPropertyChanged();
            }
        }


        private double _difference;

        public double Difference
        {
            get => _difference;
            set
            {
                if (_difference == value)
                    return;

                _difference = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<HistoryLookup, History> _historyLookupDefinition;

        public LookupDefinition<HistoryLookup, History> HistoryLookupDefinition
        {
            get => _historyLookupDefinition;
            set
            {
                if (_historyLookupDefinition == value)
                    return;

                _historyLookupDefinition = value;
                OnPropertyChanged(nameof(HistoryLookupDefinition), false);
            }
        }

        private LookupCommand _historyLookupCommand;

        public LookupCommand HistoryLookupCommand
        {
            get => _historyLookupCommand;
            set
            {
                if (_historyLookupCommand == value)
                    return;

                _historyLookupCommand = value;
                OnPropertyChanged(nameof(HistoryLookupCommand), false);
            }
        }

        public ViewModelInput ViewModelInput { get; set; }

        private PeriodHistoryTypes _mode;
        private bool _noFilters;
        private BankAccountPeriodHistory _bankPeriodHistory;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
                _mode = PeriodHistoryTypes.Monthly;
            }
            else if (LookupAddViewArgs != null &&
                     LookupAddViewArgs.InputParameter is YearlyHistoryFilter yearlyHistoryFilter)
            {
                ViewModelInput = yearlyHistoryFilter.ViewModelInput;
                _mode = PeriodHistoryTypes.Yearly;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
                _mode = PeriodHistoryTypes.Monthly;
                _noFilters = true;
            }

            ReadOnlyMode = true;

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override void
            PopulatePrimaryKeyControls(BankAccountPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _bankPeriodHistory = AppGlobals.DataRepository.GetBankPeriodHistory(newEntity.BankAccountId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            PeriodEndingDate = _bankPeriodHistory.PeriodEndingDate;

            FindButtonLookupDefinition.ReadOnlyMode = false;

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                _bankPeriodHistory.BankAccountId);



            if (_mode == PeriodHistoryTypes.Monthly)
            {
                var beginDate = new DateTime(_bankPeriodHistory.PeriodEndingDate.Year
                    , _bankPeriodHistory.PeriodEndingDate.Month, 1);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, _bankPeriodHistory.PeriodEndingDate);
            }
            else
            {
                var beginDate = new DateTime(_bankPeriodHistory.PeriodEndingDate.Year
                    , 1, 1);
                var endDate = new DateTime(_bankPeriodHistory.PeriodEndingDate.Year
                    , 12, 31);

                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, endDate);
            }

            ViewModelInput.HistoryFilterBankAccountPeriod = _bankPeriodHistory;

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

        }

        protected override BankAccountPeriodHistory GetEntityFromDb(BankAccountPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var bankItem = AppGlobals.DataRepository.GetBankAccount(_bankPeriodHistory.BankAccountId);

            BankAccount = bankItem.Description;

            return _bankPeriodHistory;
        }

        protected override void LoadFromEntity(BankAccountPeriodHistory entity)
        {
            TotalDeposits = entity.TotalDeposits;
            TotalWithdrawals = entity.TotalWithdrawals;
            Difference = TotalDeposits - TotalWithdrawals;
        }

        protected override BankAccountPeriodHistory GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            BankAccount = string.Empty;
            PeriodEndingDate = DateTime.Today;
            TotalDeposits = TotalWithdrawals = Difference = 0;

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(BankAccountPeriodHistory entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            ViewModelInput.HistoryFilterBankAccountPeriod = null;
            base.OnWindowClosing(e);
        }

        private void MakeFilters(LookupDefinitionBase lookupDefinition)
        {
            if (!_noFilters)
            {
                lookupDefinition.FilterDefinition.AddFixedFilter(
                    TableDefinition.GetFieldDefinition(p => p.BankAccountId),
                    Conditions.Equals, _bankPeriodHistory.BankAccountId);

                lookupDefinition.FilterDefinition.AddFixedFilter(
                    TableDefinition.GetFieldDefinition(p => p.PeriodType),
                    Conditions.Equals, _bankPeriodHistory.PeriodType);
            }
        }

        protected override void PrintOutput()
        {
            var printerSetup = CreatePrinterSetupArgs();

            var callBack = new HistoryPrintFilterCallBack();
            callBack.FilterDate = PeriodEndingDate;

            callBack.PrintOutput += (sender, model) =>
            {
                if (printerSetup.LookupDefinition is LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> historyLookup)
                {
                    historyLookup.FilterDefinition.ClearFixedFilters();

                    MakeFilters(printerSetup.LookupDefinition);

                    if (model.BeginningDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.PeriodEndingDate,
                            Conditions.GreaterThanEquals,
                            model.BeginningDate.Value);
                    }

                    if (model.EndingDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.PeriodEndingDate,
                            Conditions.LessThanEquals,
                            model.EndingDate.Value);
                    }
                }

                Processor.PrintOutput(printerSetup);
            };

            AppGlobals.MainViewModel.View.ShowHistoryPrintFilterWindow(callBack);
        }
    }
}
