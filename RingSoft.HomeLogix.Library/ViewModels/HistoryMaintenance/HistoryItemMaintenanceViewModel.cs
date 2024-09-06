using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using System;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class HistoryItemMaintenanceViewModel : DbMaintenanceViewModel<History>
    {
        #region Properties

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


        private double _projectedAmount;

        public double ProjectedAmount
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

        private double _actualAmount;

        public double ActualAmount
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

        private double _difference;

        public double Difference
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

        private string _bankText;

        public string BankText
        {
            get => _bankText;
            set
            {
                if (_bankText == value)
                {
                    return;
                }
                _bankText = value;
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

        #endregion

        public ViewModelInput ViewModelInput { get; set; }

        private BudgetItem _budgetItemFilter;
        private BudgetPeriodHistory _budgetPeriodHistoryFilter;
        private BankAccount _bankAccountFilter;
        private BankAccountPeriodHistory _bankAccountPeriodHistoryFilter;

        protected override void Initialize()
        {
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

            ReadOnlyMode = true;

            var sourceHistoryLookupDefinition = new LookupDefinition<SourceHistoryLookup, SourceHistory>(AppGlobals.LookupContext.SourceHistory);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Date,
                p => p.Date);
            sourceHistoryLookupDefinition.Include(p => p.Source).
                AddVisibleColumnDefinition(p => p.Source,
                p => p.Name);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Amount, p => p.Amount);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.BankText, p => p.BankText);
            SourceHistoryLookupDefinition = sourceHistoryLookupDefinition;

            BudgetAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.BudgetItemId))
            {
                AddViewParameter = ViewModelInput,
            };

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup)
            {
                AddViewParameter = ViewModelInput
            };

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override void PopulatePrimaryKeyControls(History newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            MakeFilters(ViewLookupDefinition);

            SourceHistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            SourceHistoryLookupDefinition.FilterDefinition
                .AddFixedFilter(p => p.HistoryId, Conditions.Equals, Id);

            SourceHistoryLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput));
        }

        protected override History GetEntityFromDb(History newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var historyItem = AppGlobals.DataRepository.GetHistoryItem(newEntity.Id);
            return historyItem;
        }

        private void MakeFilters(LookupDefinitionBase lookupDefinition)
        {
            lookupDefinition.FilterDefinition.ClearFixedFilters();

            if (_budgetItemFilter != null)
            {
                lookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.History.GetFieldDefinition(p => p.BudgetItemId),
                    Conditions.Equals, _budgetItemFilter.Id);
            }

            DateTime filterDate = DateTime.Today;

            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = AppGlobals.LookupContext.History;
            var formula = string.Empty;
            formula = $"{GetDateFormula(true)} {sqlGenerator.FormatSqlObject(table.TableName)}.";


            if (_budgetPeriodHistoryFilter != null)
            {
                filterDate = _budgetPeriodHistoryFilter.PeriodEndingDate;

                formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";

                lookupDefinition.FilterDefinition.AddFixedFilter("Month",
                    Conditions.Equals, $"{filterDate.Month:D2}", formula);

                formula = $"{GetDateFormula(false)} {sqlGenerator.FormatSqlObject(table.TableName)}.";
                formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";

                lookupDefinition.FilterDefinition.AddFixedFilter("Year",
                    Conditions.Equals, $"{filterDate.Year:D4}", formula);
            }

            if (_bankAccountPeriodHistoryFilter != null)
            {
                filterDate = _bankAccountPeriodHistoryFilter.PeriodEndingDate;
                formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";

                lookupDefinition.FilterDefinition.AddFixedFilter("Month",
                    Conditions.Equals, $"{filterDate.Month:D2}", formula);

                formula = $"{GetDateFormula(false)} {sqlGenerator.FormatSqlObject(table.TableName)}.";
                formula += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";

                lookupDefinition.FilterDefinition.AddFixedFilter("Year",
                    Conditions.Equals, $"{filterDate.Year:D4}", formula);
            }


            if (_bankAccountFilter != null)
            {
                lookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.History.GetFieldDefinition(p => p.BankAccountId),
                    Conditions.Equals, _bankAccountFilter.Id);
            }
        }

        private static string GetDateFormula(bool month)
        {
            string formula;
            switch (AppGlobals.DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    if (month)
                    {
                        formula = $"strftime('%m', ";
                    }
                    else
                    {
                        formula = $"strftime('%Y', ";
                    }

                    break;
                case DbPlatforms.SqlServer:
                    if (month)
                    {
                        formula = $"MONTH(";
                    }
                    else
                    {
                        formula = $"YEAR(";
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return formula;
        }

        protected override void LoadFromEntity(History entity)
        {
            Date = entity.Date;
            Description = entity.Description;
            ItemType = entity.ItemType;
            BankText = entity.BankText;


            ProjectedAmount = entity.ProjectedAmount;
            ActualAmount = entity.ActualAmount;

            if (entity.BudgetItem != null)
            {
                if ((BudgetItemTypes)entity.BudgetItem.Type == BudgetItemTypes.Income)
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

            BudgetAutoFillValue = entity.BudgetItem.GetAutoFillValue();
            BankAutoFillValue = entity.BankAccount.GetAutoFillValue();
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

            SourceHistoryLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
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

        protected override void PrintOutput()
        {
            var printerSetup = CreatePrinterSetupArgs();

            var callBack = new HistoryPrintFilterCallBack();
            callBack.FilterDate = Date;

            callBack.PrintOutput += (sender, model) =>
            {
                MakeFilters(printerSetup.LookupDefinition);

                if (printerSetup.LookupDefinition is LookupDefinition<HistoryLookup, History> historyLookup)
                {
                    if (model.BeginningDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.Date,
                            Conditions.GreaterThanEquals,
                            model.BeginningDate.Value);
                    }

                    if (model.EndingDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.Date,
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
