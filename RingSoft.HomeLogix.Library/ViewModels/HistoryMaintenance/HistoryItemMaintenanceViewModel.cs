﻿using System;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class HistoryItemMaintenanceViewModel : AppDbMaintenanceViewModel<History>
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

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

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                {
                    return;
                }
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _itemType;

        public int ItemType
        {
            get => _itemType;
            set
            {
                if (_itemType == value)
                {
                    return;
                }
                _itemType = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _budgetAutoFillSetup;

        public AutoFillSetup BudgetAutoFillSetup
        {
            get => _budgetAutoFillSetup;
            set
            {
                if (_budgetAutoFillSetup == value)
                {
                    return;
                }
                _budgetAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _budgetAutoFillValue;

        public AutoFillValue BudgetAutoFillValue
        {
            get => _budgetAutoFillValue;
            set
            {
                if (_budgetAutoFillValue == value)
                {
                    return;
                }
                _budgetAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _bankAutoFillSetup;

        public AutoFillSetup BankAutoFillSetup
        {
            get => _bankAutoFillSetup;
            set
            {
                if (_bankAutoFillSetup == value)
                    return;

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
                    return;

                _bankAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedAmount;

        public decimal ProjectedAmount
        {
            get => _projectedAmount;
            set
            {
                if (_projectedAmount == value)
                {
                    return;
                }
                _projectedAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _actualAmount;

        public decimal ActualAmount
        {
            get => _actualAmount;
            set
            {
                if (_actualAmount ==  value)
                {
                    return;
                }
                _actualAmount = value;
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
                {
                    return;
                }
                _difference = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<SourceHistoryLookup, SourceHistory> _sourceHistoryLookupDefinition;

        public LookupDefinition<SourceHistoryLookup, SourceHistory> SourceHistoryLookupDefinition
        {
            get => _sourceHistoryLookupDefinition;
            set
            {
                if (_sourceHistoryLookupDefinition == value)
                {
                    return;
                }
                _sourceHistoryLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _sourceHistoryLookupCommand;

        public LookupCommand SourceHistoryLookupCommand
        {
            get => _sourceHistoryLookupCommand;
            set
            {
                if (_sourceHistoryLookupCommand == value)
                {
                    return;
                }
                _sourceHistoryLookupCommand = value;
                OnPropertyChanged();
            }
        }


        public ViewModelInput ViewModelInput { get; set; }

        public override TableDefinition<History> TableDefinition => AppGlobals.LookupContext.History;

        private BudgetItem _budgetItemFilter;
        private BudgetPeriodHistory _budgetPeriodHistoryFilter;
        private BankAccount _bankAccountFilter;
        private BankAccountPeriodHistory _bankAccountPeriodHistoryFilter;

        protected override void Initialize()
        {
            //FindButtonLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();
            //FindButtonLookupDefinition.ReadOnlyMode = false;
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }

            _budgetItemFilter = ViewModelInput.HistoryFilterBudgetItem;
            _budgetPeriodHistoryFilter = ViewModelInput.HistoryFilterBudgetPeriodItem;
            _bankAccountFilter = ViewModelInput.HistoryFilterBankAccount;
            _bankAccountPeriodHistoryFilter = ViewModelInput.HistoryFilterBankAccountPeriod;

            ViewModelInput.HistoryFilterBudgetItem = null;
            ViewModelInput.HistoryFilterBudgetPeriodItem = null;
            ViewModelInput.HistoryFilterBankAccount = null;
            ViewModelInput.HistoryFilterBankAccountPeriod = null;

            BudgetAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BudgetItemsLookup);
            BudgetAutoFillSetup.LookupDefinition.ReadOnlyMode = true;
            ReadOnlyMode = true;

            BudgetAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.BudgetItemId))
            {
                AddViewParameter = ViewModelInput,
                //AllowLookupAdd = false
            };

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup)
            {
                AddViewParameter = ViewModelInput
            };

            var sourceHistoryLookupDefinition = new LookupDefinition<SourceHistoryLookup, SourceHistory>(AppGlobals.LookupContext.SourceHistory);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Date,
                p => p.Date);
            sourceHistoryLookupDefinition.Include(p => p.Source).
                AddVisibleColumnDefinition(p => p.Source,
                p => p.Name);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Amount, p => p.Amount);
            SourceHistoryLookupDefinition = sourceHistoryLookupDefinition;

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override History PopulatePrimaryKeyControls(History newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var historyItem = AppGlobals.DataRepository.GetHistoryItem(newEntity.Id);
            Id = newEntity.Id;

            //ViewLookupDefinition.FilterDefinition.ClearFixedFilters();
            //DbDataProcessor.ShowSqlStatementWindow();
            //if (_budgetItemFilter != null)
            //{
            //    ViewLookupDefinition.FilterDefinition.AddFixedFilter(
            //        AppGlobals.LookupContext.History.GetFieldDefinition(p => p.BudgetItemId),
            //        Conditions.Equals, _budgetItemFilter.Id);
            //}

            //DateTime filterDate = DateTime.Today;

            //var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            //var table = AppGlobals.LookupContext.History;
            //var formula = $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";


            //if (_budgetPeriodHistoryFilter != null)
            //{
            //    filterDate = _budgetPeriodHistoryFilter.PeriodEndingDate;
                
            //    formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)}) = ";
            //    formula += $"'{filterDate.Month:D2}'";

            //    ViewLookupDefinition.FilterDefinition.AddFixedFilter(formula);

            //}

            //if (_bankAccountPeriodHistoryFilter != null)
            //{
            //    filterDate= _bankAccountPeriodHistoryFilter.PeriodEndingDate;
            //    formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)}) = ";
            //    formula += $"'{filterDate.Month:D2}'";

            //    ViewLookupDefinition.FilterDefinition.AddFixedFilter(formula);
            //}

            //formula =
            //    $"strftime('%Y', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            //formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)}) = ";
            //formula += $"'{filterDate.Year:D4}'";

            //ViewLookupDefinition.FilterDefinition.AddFixedFilter(formula);

            //if (_bankAccountFilter != null)
            //{
            //    ViewLookupDefinition.FilterDefinition.AddFixedFilter(
            //        AppGlobals.LookupContext.History.GetFieldDefinition(p => p.BankAccountId), 
            //        Conditions.Equals, _bankAccountFilter.Id);
            //}

            SourceHistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            SourceHistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.HistoryId, Conditions.Equals, Id);

            SourceHistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            return historyItem;
        }

        protected override void LoadFromEntity(History entity)
        {
            Date = entity.Date;
            Description = entity.Description;
            ItemType = entity.ItemType;



            ProjectedAmount = entity.ProjectedAmount;
            ActualAmount = entity.ActualAmount;

            if (entity.BudgetItem != null)
            {
                if (entity.BudgetItem.Type == BudgetItemTypes.Income)
                    Difference = ActualAmount - ProjectedAmount;
                else
                {
                    Difference = ProjectedAmount - ActualAmount;
                }
            }
            else
            {
                Difference = ProjectedAmount - ActualAmount;
            }

            if (entity.BudgetItem != null)
            {
                BudgetAutoFillValue =
                    new AutoFillValue(
                        AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(entity.BudgetItem),
                        entity.BudgetItem.Description);
            }

            if (entity.BankAccount != null)
            {
                BankAutoFillValue =
                    new AutoFillValue(
                        AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.BankAccount),
                        entity.BankAccount.Description);
            }
        }

        protected override History GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            Id = 0;
            Description = string.Empty;
            BankAutoFillValue = null;
            BudgetAutoFillValue = null;
            ProjectedAmount = ActualAmount = Difference = 0;

            SourceHistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(History entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.SourceHistory)
            {
                var sourceHistory = AppGlobals.LookupContext.SourceHistory.GetEntityFromPrimaryKeyValue(
                    addViewPrimaryKeyValue);

                sourceHistory =
                    AppGlobals.DataRepository.GetSourceHistory(sourceHistory.HistoryId, sourceHistory.DetailId);

                return AppGlobals.LookupContext.History.GetPrimaryKeyValueFromEntity(sourceHistory.HistoryItem);
            }
            return base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
        }
    }
}
