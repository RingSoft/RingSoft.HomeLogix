﻿using System;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BankPeriodHistoryViewModel : AppDbMaintenanceViewModel<BankAccountPeriodHistory>
    {
        private AutoFillSetup _bankAutoFillSetup;

        public AutoFillSetup BankAutoFillSetup
        {
            get => _bankAutoFillSetup;
            set
            {
                if (_bankAutoFillSetup == value)
                {
                    return;
                }
                _bankAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _bankAutoFillValue;

        public AutoFillValue BankAutoFillValue
        {
            get => _bankAutoFillValue;
            set
            {
                if (_bankAutoFillValue == value)
                {
                    return;
                }
                _bankAutoFillValue = value;
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

        private decimal _totalDeposits;

        public decimal TotalDeposits
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

        private decimal _totalWithdrawals ;

        public decimal TotalWithdrawals
        {
            get => _totalWithdrawals;
            set
            {
                if (_totalWithdrawals == value)
                {
                    return;
                }
                _totalWithdrawals  = value;
                OnPropertyChanged();
            }
        }


        private decimal _difference;

        public decimal Difference
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
            }

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);
            BankAutoFillSetup.LookupDefinition.ReadOnlyMode = true;
            ReadOnlyMode = true;

            BankAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.BankAccountId))
            {
                AddViewParameter = ViewModelInput,
                //AllowLookupAdd = false
            };

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override BankAccountPeriodHistory
            PopulatePrimaryKeyControls(BankAccountPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var bankPeriodHistory = AppGlobals.DataRepository.GetBankPeriodHistory(newEntity.BankAccountId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            var bankItem = AppGlobals.DataRepository.GetBankAccount(bankPeriodHistory.BankAccountId);

            BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankItem),
                bankItem.Description);

            PeriodEndingDate = bankPeriodHistory.PeriodEndingDate;

            FindButtonLookupDefinition.FilterDefinition.AddFixedFilter(
                TableDefinition.GetFieldDefinition(p => p.BankAccountId),
                Conditions.Equals, bankPeriodHistory.BankAccountId);

            FindButtonLookupDefinition.FilterDefinition.AddFixedFilter(
                TableDefinition.GetFieldDefinition(p => p.PeriodType),
                Conditions.Equals, bankPeriodHistory.PeriodType);

            FindButtonLookupDefinition.ReadOnlyMode = false;

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                bankPeriodHistory.BankAccountId);

            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = AppGlobals.LookupContext.History;
            var sql =
                $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
            if (_mode == PeriodHistoryTypes.Monthly)
            {
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter("Month", Conditions.Equals, $"{bankPeriodHistory.PeriodEndingDate.Month:D2}", sql);
            }

            sql =
                $"strftime('%Y', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter("Year", Conditions.Equals, $"{bankPeriodHistory.PeriodEndingDate.Year:D4}", sql);

            ViewModelInput.HistoryFilterBankAccountPeriod = bankPeriodHistory;

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            return bankPeriodHistory;

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
            BankAutoFillValue = null;
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

        public override TableDefinition<BankAccountPeriodHistory> TableDefinition =>
            AppGlobals.LookupContext.BankAccountPeriodHistory;
    }
}