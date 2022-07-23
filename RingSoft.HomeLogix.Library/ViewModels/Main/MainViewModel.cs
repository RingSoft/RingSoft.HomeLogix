using System;
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

            if (loadVm)
            {
                BudgetLookupDefinition = CreateBudgetLookupDefinition();
                SetStartupView();
            }
        }

        private void SetStartupView()
        {
            var currentBudgetMonth = AppGlobals.DataRepository.GetMaxMonthBudgetPeriodHistory();
            if (currentBudgetMonth == null)
            {
                RefreshView();
            }
            else
            {
                SetCurrentMonthEnding(currentBudgetMonth.PeriodEndingDate);
            }
        }

        private LookupDefinition<MainBudgetLookup, BudgetItem> CreateBudgetLookupDefinition()
        {
            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var budgetLookupDefinition =
                new LookupDefinition<MainBudgetLookup, BudgetItem>(AppGlobals.LookupContext.BudgetItems);
            //var table = AppGlobals.LookupContext.BankAccountRegisterItems;

            budgetLookupDefinition.AddVisibleColumnDefinition(p => p.Description, "Description");
            budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ItemType, p => p.Type);

            var sql = "SELECT [BudgetItems].[Id] AS Id, \r\n";
            sql += "[BudgetItems].[Description] AS Description, \r\n";
            sql += "BudgetItems.Type AS Type,\r\n";
            //sql += "(\r\n";
            //sql += "CASE [BudgetItems].[Type]\r\n";
            //sql += "WHEN 0 THEN 'Income'\r\n";
            //sql += "WHEN 1 THEN 'Expense'\r\n";
            //sql += "WHEN 2 THEN 'Transfer'\r\n";
            //sql += "END\r\n";
            //sql += ") AS [Type],\r\n";
            sql += $"{GetBudgetMonthToDateFormulaSql()} AS Projected,\r\n";
            sql += $"({GetBudgetMonthToDateFormulaSql(false)}) AS Actual,\r\n";
            sql += $"({GetBudgetMonthlyAmountDifferenceFormulaSql()}) AS Difference\r\n";
            sql += $"FROM BudgetItems AS BudgetItems\r\n";
            sql += "WHERE Projected <> 0\r\n";

            budgetLookupDefinition.HasFromFormula(sql);

            _projectedMonthToDateColumnDefinition = budgetLookupDefinition.AddVisibleColumnDefinition(
                p => p.ProjectedMonthlyAmount, "Projected");
            _projectedMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

            //sql = GetBudgetMonthToDateFormulaSql(false);

            _actualMonthToDateColumnDefinition =
                budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ActualMonthlyAmount, "Actual");
            _actualMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

            //sql = GetBudgetMonthlyAmountDifferenceFormulaSql();
            _monthlyAmountDifferrenceColumnDefinition =
                budgetLookupDefinition.AddVisibleColumnDefinition(p => p.MonthlyAmountDifference, "Difference");
            _monthlyAmountDifferrenceColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasKeepNullEmpty()
                .DoShowNegativeValuesInRed();

            //DbDataProcessor.ShowSqlStatementWindow(true);

            return budgetLookupDefinition;
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
            
            var unionSql = $"{sql}\r\n";
            unionSql += $"\r\nUNION ALL\r\n\r\n";
            unionSql += $"SELECT coalesce(SUM(abs({sqlGenerator.FormatSqlObject(table.TableName)}.";
            unionSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ProjectedAmount).FieldName)})),0) AS Projected\r\n";
            unionSql += $"FROM {sqlGenerator.FormatSqlObject(table.TableName)}\r\n";
            unionSql += $"WHERE {sqlGenerator.FormatSqlObject(table.TableName)}.{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)} = ";
            unionSql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}\r\n ";
            unionSql += $"AND strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
            unionSql += $"'{CurrentMonthEnding.Month:D2}'";

            var projectedSql = $"(\r\nSELECT SUM(Projected) AS Projected FROM( {unionSql})\r\n)";
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
            //query.AddSelectFormulaColumn(
            //    field,
            //    $"{sqlGenerator.FormatSqlObject(table.TableName)}.{sqlGenerator.FormatSqlObject(field)}");
            //query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodType).FieldName, Conditions.Equals,
            //    (int)PeriodHistoryTypes.Monthly);
            //query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodEndingDate).FieldName, Conditions.Equals,
            //    CurrentMonthEnding, DbDateTypes.DateOnly);

            query.AddSelectFormulaColumn(sumField,
                $"(coalesce(SUM({sqlGenerator.FormatSqlObject(table.TableName)}.{field}), 0)) ");
            query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodType).FieldName, Conditions.Equals,
                (int)PeriodHistoryTypes.Monthly);
            query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodEndingDate).FieldName, Conditions.Equals,
                CurrentMonthEnding, DbDateTypes.DateOnly);

            var formulaSql = sqlGenerator.GenerateSelectStatement(query);
            formulaSql +=
                $"\r\nAND ";
            formulaSql += $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
            formulaSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)} = ";
            formulaSql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";

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
            BudgetLookupCommand = new LookupCommand(LookupCommands.Refresh);

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

        private void SetCurrentMonthEnding(DateTime value)
        {
            CurrentMonthEnding = GetPeriodEndDate(value);
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
