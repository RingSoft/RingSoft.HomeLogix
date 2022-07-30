﻿using System;
using RingSoft.DataEntryControls.Engine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public interface IMainView
    {
        bool ChangeHousehold();

        void ManageBudget();

        void ManageBankAccounts();
    }

    public class MainViewModel : INotifyPropertyChanged, IMainViewModel
    {
        private DateTime _currentMonthEnding;

        public DateTime CurrentMonthEnding
        {
            get => _currentMonthEnding;
            set
            {
                _currentMonthEnding = value;
                CurrentMonthEndingText = CurrentMonthEnding.ToString("MMMM yyyy");
            }
        }

        private string _currentMonthEndingText;

        public string CurrentMonthEndingText
        {
            get => _currentMonthEndingText;
            set
            {
                if (_currentMonthEndingText == value)
                    return;

                _currentMonthEndingText = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<MainBudgetLookup, BudgetItem> _budgetLookupDefinition;

        public LookupDefinition<MainBudgetLookup, BudgetItem> BudgetLookupDefinition
        {
            get => _budgetLookupDefinition;
            set
            {
                if (_budgetLookupDefinition == value)
                    return;

                _budgetLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _budgetLookupCommand;

        public LookupCommand BudgetLookupCommand
        {
            get => _budgetLookupCommand;
            set
            {
                if (_budgetLookupCommand == value)
                    return;

                _budgetLookupCommand = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<MainBankLookup, BankAccount> _bankLookupDefinition;

        public LookupDefinition<MainBankLookup, BankAccount> BankLookupDefinition
        {
            get => _bankLookupDefinition;
            set
            {
                if (_bankLookupDefinition == value)
                {
                    return;
                }

                _bankLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _bankLookupCommand;

        public LookupCommand BankLookupCommand
        {
            get => _bankLookupCommand;
            set
            {
                if (_bankLookupCommand == value)
                {
                    return;
                }
                _bankLookupCommand = value;
                OnPropertyChanged();
            }
        }



        private decimal _totalProjectedMonthlyIncome;

        public decimal TotalProjectedMonthlyIncome
        {
            get => _totalProjectedMonthlyIncome;
            set
            {
                if (_totalProjectedMonthlyIncome == value)
                    return;

                _totalProjectedMonthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalProjectedMonthlyExpenses;

        public decimal TotalProjectedMonthlyExpenses
        {
            get => _totalProjectedMonthlyExpenses;
            set
            {
                if (_totalProjectedMonthlyExpenses == value)
                    return;

                _totalProjectedMonthlyExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalBudgetMonthlyNetIncome;

        public decimal TotalBudgetMonthlyNetIncome
        {
            get => _totalBudgetMonthlyNetIncome;
            set
            {
                if (_totalBudgetMonthlyNetIncome == value)
                    return;

                _totalBudgetMonthlyNetIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalActualMonthlyIncome;

        public decimal TotalActualMonthlyIncome
        {
            get => _totalActualMonthlyIncome;
            set
            {
                if (_totalActualMonthlyIncome == value)
                    return;

                _totalActualMonthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalActualMonthlyExpenses;

        public decimal TotalActualMonthlyExpenses
        {
            get => _totalActualMonthlyExpenses;
            set
            {
                if (_totalActualMonthlyExpenses == value)
                    return;

                _totalActualMonthlyExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalActualMonthlyNetIncome;

        public decimal TotalActualMonthlyNetIncome
        {
            get => _totalActualMonthlyNetIncome;
            set
            {
                if (_totalActualMonthlyNetIncome == value)
                    return;

                _totalActualMonthlyNetIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMonthlyIncomeDifference;

        public decimal TotalMonthlyIncomeDifference
        {
            get => _totalMonthlyIncomeDifference;
            set
            {
                if (_totalMonthlyIncomeDifference == value)
                    return;

                _totalMonthlyIncomeDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMonthlyExpensesDifference;

        public decimal TotalMonthlyExpensesDifference
        {
            get => _totalMonthlyExpensesDifference;
            set
            {
                if (_totalMonthlyExpensesDifference == value)
                    return;

                _totalMonthlyExpensesDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMonthlyNetIncomeDifference;

        public decimal TotalMonthlyNetIncomeDifference
        {
            get => _totalMonthlyNetIncomeDifference;
            set
            {
                if (_totalMonthlyNetIncomeDifference == value)
                    return;

                _totalMonthlyNetIncomeDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _yearToDateIncome;

        public decimal YearToDateIncome
        {
            get => _yearToDateIncome;
            set
            {
                if (_yearToDateIncome == value)
                    return;

                _yearToDateIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _yearToDateExpenses;

        public decimal YearToDateExpenses
        {
            get => _yearToDateExpenses;
            set
            {
                if (_yearToDateExpenses == value)
                    return;

                _yearToDateExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _yearToDateNetIncome;

        public decimal YearToDateNetIncome
        {
            get => _yearToDateNetIncome;
            set
            {
                if (_yearToDateNetIncome == value)
                    return;

                _yearToDateNetIncome = value;
                OnPropertyChanged();
            }
        }

        public IMainView View { get; private set; }

        public RelayCommand PreviousMonthCommand { get; }
        public RelayCommand NextMonthCommand { get; }
        public RelayCommand ChangeHouseholdCommand { get; }
        public RelayCommand ManageBudgetCommand { get; }
        public RelayCommand ManageBankAccountsCommand { get; }

        private LookupFormulaColumnDefinition _projectedMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _actualMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _monthlyAmountDifferrenceColumnDefinition;

        public MainViewModel()
        {
            CurrentMonthEnding = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            PreviousMonthCommand = new RelayCommand(GotoPreviousMonth);
            NextMonthCommand = new RelayCommand(GotoNextMonth);
            ChangeHouseholdCommand = new RelayCommand(ChangeHousehold);
            ManageBudgetCommand = new RelayCommand(ManageBudget);
            ManageBankAccountsCommand = new RelayCommand(ManageBankAccounts);
        }

        public void OnViewLoaded(IMainView view)
        {
            View = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInHousehold == null)
                loadVm = View.ChangeHousehold();

            BankLookupDefinition = CreateBankLookupDefinition();
            if (loadVm)
            {
                SetStartupView();
            }
        }

        private void SetStartupView()
        {
            var currentBudgetMonth = AppGlobals.DataRepository.GetMaxMonthBudgetPeriodHistory();
            if (currentBudgetMonth == null)
            {
                CreateBudgetLookupDefinition(true);
                RefreshView();
            }
            else
            {
                SetCurrentMonthEnding(currentBudgetMonth.PeriodEndingDate, false);
                //BankLookupDefinition = CreateBankLookupDefinition();
                CreateBudgetLookupDefinition(true);
                RefreshView();
            }
        }

        private LookupDefinition<MainBankLookup, BankAccount> CreateBankLookupDefinition()
        {
            var bankLookupDefinition =
                new LookupDefinition<MainBankLookup, BankAccount>(AppGlobals.LookupContext.BankAccounts);

            bankLookupDefinition.AddVisibleColumnDefinition(p => p.Description, 
                p => p.Description);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.CurrentBalance, 
                p => p.CurrentBalance);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedLowestBalance,
                p => p.ProjectedLowestBalanceAmount);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedLowestBalanceDate,
                p => p.ProjectedLowestBalanceDate);
            return bankLookupDefinition;
        }

        private void CreateBudgetLookupDefinition(bool createColumns)
        {
            var budgetLookupDefinition = BudgetLookupDefinition;
            
            if (createColumns)
            {
                if (budgetLookupDefinition == null)
                    budgetLookupDefinition =
                        new LookupDefinition<MainBudgetLookup, BudgetItem>(AppGlobals.LookupContext.BudgetItems);

                budgetLookupDefinition.AddVisibleColumnDefinition(p => p.Description, "Description");
                budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ItemType, p => p.Type);

                _projectedMonthToDateColumnDefinition = budgetLookupDefinition.AddVisibleColumnDefinition(
                    p => p.ProjectedMonthlyAmount, "Projected");
                _projectedMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

                _actualMonthToDateColumnDefinition =
                    budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ActualMonthlyAmount, "Actual");
                _actualMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

                _monthlyAmountDifferrenceColumnDefinition =
                    budgetLookupDefinition.AddVisibleColumnDefinition(p => p.MonthlyAmountDifference, "Difference");
                _monthlyAmountDifferrenceColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency)
                    .HasKeepNullEmpty()
                    .DoShowNegativeValuesInRed();

                BudgetLookupDefinition = budgetLookupDefinition;
            }

            budgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
        }
        private string CreateBudgetLookupDefinitionFormula()
        {
            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = AppGlobals.LookupContext.BudgetItems;
            var query = new SelectQuery(table.TableName);

            query.AddSelectColumn(table.GetFieldDefinition(p => p.Id).FieldName, "Id");
            query.AddSelectColumn(table.GetFieldDefinition(p => p.Description).FieldName, "Description");
            query.AddSelectFormulaColumn("Type", $"{sqlGenerator.FormatSqlObject(table.TableName)}.{sqlGenerator.FormatSqlObject("Type")}");

            query.AddSelectFormulaColumn("Projected", GetBudgetMonthToDateFormulaSql());
            query.AddSelectFormulaColumn("Actual", GetBudgetMonthToDateFormulaSql(false));
            query.AddSelectFormulaColumn("Difference", GetBudgetMonthlyAmountDifferenceFormulaSql());

            query.AddWhereItemFormula("Projected", Conditions.NotEquals, (int) 0).SetEndLogic(EndLogics.Or);
            query.AddWhereItemFormula("Actual", Conditions.NotEquals, (int)0);

            return sqlGenerator.GenerateSelectStatement(query);
        }

        private string GetBudgetMonthlyAmountDifferenceFormulaSql()
        {
            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName);
            var typeField = sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems
                .GetFieldDefinition(p => (int) p.Type).FieldName);

            var result =
                $"CASE {table}.{typeField} WHEN {(int) BudgetItemTypes.Income} THEN ({GetBudgetMonthToDateFormulaSql(false)})"
                + $" - ({GetBudgetMonthToDateFormulaSql()}) "
                + $"ELSE ({GetBudgetMonthToDateFormulaSql()}) - ({GetBudgetMonthToDateFormulaSql(false)}) END";

            return result;
        }

        private string GetBudgetMonthToDateFormulaSql()
        {
            var table = AppGlobals.LookupContext.BankAccountRegisterItems;
            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;

            var sql = GetBudgetMonthToDateFormulaSql(true);
            var query = new SelectQuery(table.TableName);
            
            var unionSql = $"{sql}\r\n";
            unionSql += $"\r\nUNION ALL\r\n\r\n";

            sql = $"coalesce(SUM(abs({sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ProjectedAmount).FieldName)}";
            sql += ")),0)";
            query.AddSelectFormulaColumn("Projected",sql);

            sql = $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)} = ";
            sql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.";
            sql += $"{ sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";
            query.AddWhereItemFormula(sql);

            sql =
                $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
            sql += $"'{CurrentMonthEnding.Month:D2}'";
            query.AddWhereItemFormula(sql);

            unionSql += sqlGenerator.GenerateSelectStatement(query);

            var outerQuery = new SelectQuery("Outer");
            outerQuery.AddSelectFormulaColumn("Projected", "SUM(Projected)");
            outerQuery.BaseTable.HasFormula(unionSql + "\r\n");

            var projectedSql = sqlGenerator.GenerateSelectStatement(outerQuery);

            return projectedSql;
        }

        private string GetBudgetMonthToDateFormulaSql(bool projected)
        {
            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var query = new SelectQuery(AppGlobals.LookupContext.BudgetPeriodHistory.TableName);
            var table = AppGlobals.LookupContext.BudgetPeriodHistory;

            var field = projected
                ? AppGlobals.LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName
                : AppGlobals.LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName;

            var sumField = projected
                ? "Projected"
                : "Actual";

            query.AddSelectFormulaColumn(sumField,
                $"(coalesce(SUM({sqlGenerator.FormatSqlObject(table.TableName)}.{field}), 0)) ");
            query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodType).FieldName, Conditions.Equals,
                (int)PeriodHistoryTypes.Monthly);
            query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodEndingDate).FieldName, Conditions.Equals,
                CurrentMonthEnding, DbDateTypes.DateOnly);

            var formulaSql = $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
            formulaSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)}";

            var equalsClause = $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.";
            equalsClause += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";

            query.AddWhereItemFormula($"{formulaSql} = {equalsClause}");

            formulaSql = sqlGenerator.GenerateSelectStatement(query);

            return formulaSql;
        }

        private void ChangeHousehold()
        {
            if (View.ChangeHousehold())
            {
                SetStartupView();
            }
        }

        public void RefreshView()
        {
            BudgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
            BudgetLookupCommand = new LookupCommand(LookupCommands.Refresh);
            BankLookupCommand = new LookupCommand(LookupCommands.Refresh);

            var budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(CurrentMonthEnding,
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(-1)), 
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(1)));

            TotalProjectedMonthlyIncome = budgetTotals.TotalProjectedMonthlyIncome;
            TotalProjectedMonthlyExpenses = budgetTotals.TotalProjectedMonthlyExpenses;
            TotalBudgetMonthlyNetIncome = TotalProjectedMonthlyIncome - TotalProjectedMonthlyExpenses;

            TotalActualMonthlyIncome = budgetTotals.TotalActualMonthlyIncome;
            TotalActualMonthlyExpenses = budgetTotals.TotalActualMonthlyExpenses;
            TotalActualMonthlyNetIncome = TotalActualMonthlyIncome - TotalActualMonthlyExpenses;

            TotalMonthlyIncomeDifference = TotalActualMonthlyIncome - TotalProjectedMonthlyIncome;
            TotalMonthlyExpensesDifference = TotalProjectedMonthlyExpenses - TotalActualMonthlyExpenses;
            TotalMonthlyNetIncomeDifference = TotalActualMonthlyNetIncome - TotalBudgetMonthlyNetIncome;

            YearToDateIncome = budgetTotals.YearToDateIncome;
            YearToDateExpenses = budgetTotals.YearToDateExpenses;
            YearToDateNetIncome = YearToDateIncome - YearToDateExpenses;

            PreviousMonthCommand.IsEnabled = budgetTotals.PreviousMonthHasValues;
            NextMonthCommand.IsEnabled = budgetTotals.NextMonthHasValues;
        }

        private void ManageBudget()
        {
            View.ManageBudget();
            RefreshView();
        }

        private void ManageBankAccounts()
        {
            View.ManageBankAccounts();
        }

        private void GotoPreviousMonth()
        {
            SetCurrentMonthEnding(CurrentMonthEnding.AddMonths(-1));
        }

        private void GotoNextMonth()
        {
            SetCurrentMonthEnding(CurrentMonthEnding.AddMonths(1));
        }

        private void SetCurrentMonthEnding(DateTime value, bool refreshView = true)
        {
            CurrentMonthEnding = GetPeriodEndDate(value);

            if (refreshView)
                RefreshView();
        }

        private static DateTime GetPeriodEndDate(DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
