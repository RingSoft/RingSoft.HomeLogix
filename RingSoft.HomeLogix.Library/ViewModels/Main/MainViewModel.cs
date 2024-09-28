using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetData
    {
        public MainBudget BudgetObj { get; set; }

        public string Description { get; set; }
    }

    public class ChangeDateData
    {
        public DateTime NewDate { get; set; }

        public bool DialogResult { get; set; }
    }
    public interface IMainView
    {
        bool ChangeHousehold();

        void ManageBudget();

        void ManageBankAccounts();

        void LaunchAdvancedFind();

        void CloseApp();

        void ShowChart(bool show = true);

        Login ShowPhoneSync(Login input);

        void ShowRichMessageBox(string message, string caption, RsMessageBoxIcons icon, List<HyperlinkData> hyperLinks = null);

        string GetWriteablePath();

        bool UpgradeVersion();

        void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack);

        void ShowAbout();

        void GetChangeDate(ChangeDateData data);
    }

    public class MainViewModel : INotifyPropertyChanged, IMainViewModel, ILookupControl
    {
        #region Properties

        private DateTime _currentMonthEnding;

        public DateTime CurrentMonthEnding
        {
            get => _currentMonthEnding;
            set
            {
                _currentMonthEnding = value;
                if (_setCurrentDateText)
                {
                    CurrentMonthEndingText = CurrentMonthEnding.ToString("MMMM yyyy");
                }
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

        private LookupDefinition<MainBudgetLookup, MainBudget> _budgetLookupDefinition;

        public LookupDefinition<MainBudgetLookup, MainBudget> BudgetLookupDefinition
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


        private double _totalProjectedMonthlyIncome;

        public double TotalProjectedMonthlyIncome
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

        private double _totalProjectedMonthlyExpenses;

        public double TotalProjectedMonthlyExpenses
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

        private double _totalBudgetMonthlyNetIncome;

        public double TotalBudgetMonthlyNetIncome
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

        private double _totalActualMonthlyIncome;

        public double TotalActualMonthlyIncome
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

        private double _totalActualMonthlyExpenses;

        public double TotalActualMonthlyExpenses
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

        private double _totalActualMonthlyNetIncome;

        public double TotalActualMonthlyNetIncome
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

        private double _totalMonthlyIncomeDifference;

        public double TotalMonthlyIncomeDifference
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

        private double _totalMonthlyExpensesDifference;

        public double TotalMonthlyExpensesDifference
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

        private double _totalMonthlyNetIncomeDifference;

        public double TotalMonthlyNetIncomeDifference
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

        private double _yearToDateIncome;

        public double YearToDateIncome
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

        private double _yearToDateExpenses;

        public double YearToDateExpenses
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

        private double _yearToDateNetIncome;

        public double YearToDateNetIncome
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

        private ChartData _budgetChartData;
        public ChartData BudgetChartData
        {
            get => _budgetChartData;
            set
            {
                if (_budgetChartData == value)
                {
                    return;
                }
                _budgetChartData = value;
                OnPropertyChanged();
            }
        }

        private ChartData _actualChartData;
        public ChartData ActualChartData
        {
            get => _activeChartData;
            set
            {
                if (_actualChartData == value)
                {
                    return;
                }
                _actualChartData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public IMainView View { get; private set; }

        public RelayCommand CloseAppCommand { get; private set; }
        public RelayCommand PreviousMonthCommand { get; }
        public RelayCommand NextMonthCommand { get; }
        public RelayCommand ChangeHouseholdCommand { get; }
        public RelayCommand ManageBudgetCommand { get; }
        public RelayCommand ManageBankAccountsCommand { get; }
        public RelayCommand SyncPhoneCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        public RelayCommand UpgradeCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand ChangeDateCommand { get; }

        public void SetLookupIndex(int index)
        {
        }

        public int PageSize { get; } = 20;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
        public int SelectedIndex { get; }

        public DateTime CurrentMonth { get; private set; }

        public List<BudgetItemViewModel> BudgetItemViewModels { get; } = new List<BudgetItemViewModel>();

        public List<BankAccountViewModel> BankAccountViewModels { get; } = new List<BankAccountViewModel>();

        private LookupFormulaColumnDefinition _projectedMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _actualMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _monthlyAmountDifferrenceColumnDefinition;
        private ChartData _initialBudgetChartData;
        private ChartData _initialActualChartData;
        private ChartData _activeChartData;
        private List<int> _budgetItems = new List<int>();
        private bool _setCurrentDateText = true;

        public MainViewModel()
        {
            CurrentMonthEnding = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            PreviousMonthCommand = new RelayCommand(GotoPreviousMonth);
            NextMonthCommand = new RelayCommand(GotoNextMonth);
            ChangeHouseholdCommand = new RelayCommand(ChangeHousehold);
            ManageBudgetCommand = new RelayCommand(ManageBudget);
            ManageBankAccountsCommand = new RelayCommand(ManageBankAccounts);
            ChangeDateCommand = new RelayCommand(ChangeDate);
            AdvancedFindCommand = new RelayCommand(LaunchAdvancedFind);
            CloseAppCommand = new RelayCommand(CloseApp);
            SyncPhoneCommand = new RelayCommand(SyncPhone);
            UpgradeCommand = new RelayCommand(() =>
            {
                if (!View.UpgradeVersion())
                {
                    var message = "You are already on the latest version.";
                    var caption = "Upgrade Not Necessary";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                }
            });
            AboutCommand = new RelayCommand((() =>
            {
                View.ShowAbout();
            }));
        }

        public void OnViewLoaded(IMainView view)
        {
            View = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInHousehold == null)
            {
                View.ShowChart(false);
                loadVm = View.ChangeHousehold();
            }

            if (loadVm)
            {
                BankLookupDefinition = CreateBankLookupDefinition();
                BudgetLookupDefinition = AppGlobals.LookupContext.MainBudgetLookup.Clone();
                SetStartupView();
            }
        }

        private void SetStartupView()
        {
            var look = AppGlobals.LookupContext;
            var currentBudgetMonth = AppGlobals.DataRepository.GetMaxMonthBudgetPeriodHistory();
            if (currentBudgetMonth == null)
            {
                var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                    DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

                SetCurrentMonthEnding(date, false);
                CurrentMonth = date;
                //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
                RefreshView();
            }
            else
            {
                SetCurrentMonthEnding(currentBudgetMonth.PeriodEndingDate, false);
                CurrentMonth = currentBudgetMonth.PeriodEndingDate;
                //BankLookupDefinition = CreateBankLookupDefinition();
                //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
                RefreshView();
            }
        }

        private LookupDefinition<MainBankLookup, BankAccount> CreateBankLookupDefinition()
        {
            var bankLookupDefinition =
                new LookupDefinition<MainBankLookup, BankAccount>(AppGlobals.LookupContext.BankAccounts);

            bankLookupDefinition.AddHiddenColumn(p => p.BankId, p => p.Id);

            bankLookupDefinition.AddHiddenColumn(p => p.AccountType, p => p.AccountType);

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

        //private LookupDefinition<MainBudgetLookup, MainBudget> CreateBudgetLookupDefinition()
        //{
        //    return AppGlobals.LookupContext.MainBudgetLookup.Clone();
        //    var budgetLookupDefinition = BudgetLookupDefinition;
            
        //    if (createColumns)
        //    {
        //        if (budgetLookupDefinition == null)
        //            budgetLookupDefinition =
        //                new LookupDefinition<MainBudgetLookup, BudgetItem>(AppGlobals.LookupContext.BudgetItems);

        //        budgetLookupDefinition.AddHiddenColumn(p => p.BudgetId, p => p.Id);
        //        budgetLookupDefinition.AddVisibleColumnDefinition(p => p.Description, "Description","");
        //        budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ItemType, p => p.Type);

        //        _projectedMonthToDateColumnDefinition = budgetLookupDefinition.AddVisibleColumnDefinition(
        //            p => p.ProjectedMonthlyAmount, "Projected", "");
        //        _projectedMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

        //        _actualMonthToDateColumnDefinition =
        //            budgetLookupDefinition.AddVisibleColumnDefinition(p => p.ActualMonthlyAmount, "Actual", "");
        //        _actualMonthToDateColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency);//.HasKeepNullEmpty();

        //        _monthlyAmountDifferrenceColumnDefinition =
        //            budgetLookupDefinition.AddVisibleColumnDefinition(p => p.MonthlyAmountDifference, "Difference", "");
        //        _monthlyAmountDifferrenceColumnDefinition.HasDecimalFieldType(DecimalFieldTypes.Currency)
        //            .HasKeepNullEmpty()
        //            .DoShowNegativeValuesInRed()
        //            .DoShowPositiveValuesInGreen();

        //        //BudgetLookupDefinition = budgetLookupDefinition;
        //    }

        //    budgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
            
        //    return budgetLookupDefinition;
        //}



        //private string CreateBudgetLookupDefinitionFormula()
        //{
        //    var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
        //    var table = AppGlobals.LookupContext.BudgetItems;
        //    var query = new SelectQuery(table.TableName);
        //    var outerQuery = new SelectQuery(table.TableName, query);

        //    outerQuery.AddSelectColumn("Id", "Id");
        //    outerQuery.AddSelectColumn("Description", "Description");
        //    outerQuery.AddSelectColumn("Type", "Type");
        //    outerQuery.AddSelectColumn("Projected", "Projected");
        //    outerQuery.AddSelectColumn("Actual", "Actual");

        //    var formula = $"CASE WHEN {sqlGenerator.FormatSqlObject("Actual")}=0 THEN 0 ELSE ";
        //    formula += $"{sqlGenerator.FormatSqlObject("Difference")} END";
        //    outerQuery.AddSelectFormulaColumn("Difference", formula);

        //    query.AddSelectColumn(table.GetFieldDefinition(p => p.Id).FieldName, "Id");
        //    query.AddSelectColumn(table.GetFieldDefinition(p => p.Description).FieldName, "Description");
        //    query.AddSelectFormulaColumn("Type", $"{sqlGenerator.FormatSqlObject(table.TableName)}.{sqlGenerator.FormatSqlObject("Type")}");

        //    query.AddSelectFormulaColumn("Projected", GetBudgetMonthToDateFormulaSql());
        //    query.AddSelectFormulaColumn("Actual", GeActualtMonthToDateFormulaSql());
        //    query.AddSelectFormulaColumn("Difference", GetBudgetMonthlyAmountDifferenceFormulaSql());

        //    //query.AddWhereItemFormula("Projected", Conditions.NotEquals, (int)0).SetEndLogic(EndLogics.Or);
        //    //query.AddWhereItemFormula("Actual", Conditions.NotEquals, (int)0);

        //    outerQuery.AddWhereItemFormula("Projected", Conditions.NotEquals, (int) 0).SetEndLogic(EndLogics.Or)
        //        .SetLeftParenthesesCount(1);
        //    outerQuery.AddWhereItemFormula("Actual", Conditions.NotEquals, (int)0).SetRightParenthesesCount(1);
        //    outerQuery.AddWhereItem("Type", Conditions.NotEquals, (int) BudgetItemTypes.Transfer);

        //    var sql = sqlGenerator.GenerateSelectStatement(outerQuery);
        //    return sql;
        //}

        //private string GetBudgetMonthlyAmountDifferenceFormulaSql()
        //{
        //    var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
        //    var typeField = AppGlobals.LookupContext.BudgetItems
        //        .GetFieldDefinition(p => (int)p.Type);

        //    var result =
        //        $"CASE {typeField.GetSqlFormatObject()} WHEN {(int) BudgetItemTypes.Income} THEN ({GeActualtMonthToDateFormulaSql()})"
        //        + $" - ({GetBudgetMonthToDateFormulaSql()}) "
        //        + $"ELSE ({GetBudgetMonthToDateFormulaSql()}) - ({GeActualtMonthToDateFormulaSql()}) END";

        //    return result;
        //}

        //private string GeActualtMonthToDateFormulaSql()
        //{
        //    var table = AppGlobals.LookupContext.BankAccountRegisterItems;
        //    var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;

        //    var sql = GetBudgetMonthToDateFormulaSql(false);
        //    var query = new SelectQuery(table.TableName);

        //    var unionSql = $"{sql}\r\n";
        //    unionSql += $"\r\nUNION ALL\r\n\r\n";

        //    sql = $"ROUND(coalesce(SUM(abs({sqlGenerator.FormatSqlObject(table.TableName)}.";
        //    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ProjectedAmount).FieldName)}";
        //    sql += ")),0), 2)";
        //    query.AddSelectFormulaColumn("Actual", sql);

        //    sql = $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
        //    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)} = ";
        //    sql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.";
        //    sql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";
        //    query.AddWhereItemFormula(sql);

        //    switch (AppGlobals.DbPlatform)
        //    {
        //        case DbPlatforms.SqlServer:
        //            sql =
        //                $"MONTH({sqlGenerator.FormatSqlObject(table.TableName)}.";
        //            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
        //            sql += $"'{CurrentMonthEnding.Month:D2}'";
        //            break;
        //        case DbPlatforms.Sqlite:
        //        case DbPlatforms.MySql:
        //            sql =
        //                $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
        //            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
        //            sql += $"'{CurrentMonthEnding.Month:D2}'";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //    query.AddWhereItemFormula(sql);

        //    unionSql += sqlGenerator.GenerateSelectStatement(query);

        //    var outerQuery = new SelectQuery("Outer");
        //    outerQuery.AddSelectFormulaColumn("Actual", "SUM(Actual)");
        //    outerQuery.BaseTable.HasFormula(unionSql + "\r\n");

        //    var projectedSql = sqlGenerator.GenerateSelectStatement(outerQuery);

        //    return projectedSql;
        //}


        //private string GetBudgetMonthToDateFormulaSql()
        //{
        //    var table = AppGlobals.LookupContext.BankAccountRegisterItems;
        //    var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;

        //    var sql = GetBudgetMonthToDateFormulaSql(true);
        //    var query = new SelectQuery(table.TableName);
            
        //    var unionSql = $"{sql}\r\n";
        //    unionSql += $"\r\nUNION ALL\r\n\r\n";

        //    sql = $"ROUND(coalesce(SUM(abs({sqlGenerator.FormatSqlObject(table.TableName)}.";
        //    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ProjectedAmount).FieldName)}";
        //    sql += ")),0), 2)";
        //    query.AddSelectFormulaColumn("Projected",sql);

        //    sql = $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
        //    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)} = ";
        //    sql += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.";
        //    sql += $"{ sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";
        //    query.AddWhereItemFormula(sql);

        //    switch (AppGlobals.DbPlatform)
        //    {
        //        case DbPlatforms.SqlServer:
        //            sql =
        //                $"MONTH({sqlGenerator.FormatSqlObject(table.TableName)}.";
        //            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
        //            sql += $"'{CurrentMonthEnding.Month:D2}'";
        //            break;
        //        case DbPlatforms.Sqlite:
        //        case DbPlatforms.MySql:
        //            sql =
        //                $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
        //            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.ItemDate).FieldName)}) = ";
        //            sql += $"'{CurrentMonthEnding.Month:D2}'";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    query.AddWhereItemFormula(sql);

        //    unionSql += sqlGenerator.GenerateSelectStatement(query);

        //    var outerQuery = new SelectQuery("Outer");
        //    outerQuery.AddSelectFormulaColumn("Projected", "SUM(Projected)");
        //    outerQuery.BaseTable.HasFormula(unionSql + "\r\n");

        //    var projectedSql = sqlGenerator.GenerateSelectStatement(outerQuery);

        //    return projectedSql;
        //}

        //private string GetBudgetMonthToDateFormulaSql(bool projected)
        //{
        //    var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
        //    var query = new SelectQuery(AppGlobals.LookupContext.BudgetPeriodHistory.TableName);
        //    var table = AppGlobals.LookupContext.BudgetPeriodHistory;

        //    var field = projected
        //        ? AppGlobals.LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName
        //        : AppGlobals.LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName;

        //    var sumField = projected
        //        ? "Projected"
        //        : "Actual";

        //    query.AddSelectFormulaColumn(sumField,
        //        $"ROUND((coalesce(SUM({sqlGenerator.FormatSqlObject(table.TableName)}.{field}), 0)), 2) ");
        //    query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodType).FieldName, Conditions.Equals,
        //        (int)PeriodHistoryTypes.Monthly);
        //    query.AddWhereItem(table.GetFieldDefinition(p => p.PeriodEndingDate).FieldName, Conditions.Equals,
        //        CurrentMonthEnding, DbDateTypes.DateOnly);

        //    var formulaSql = $"{sqlGenerator.FormatSqlObject(table.TableName)}.";
        //    formulaSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.BudgetItemId).FieldName)}";

        //    var equalsClause = $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.TableName)}.";
        //    equalsClause += $"{sqlGenerator.FormatSqlObject(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.Id).FieldName)}";

        //    query.AddWhereItemFormula($"{formulaSql} = {equalsClause}");

        //    formulaSql = sqlGenerator.GenerateSelectStatement(query);

        //    return formulaSql;
        //}

        private void ChangeHousehold()
        {
            View.ShowChart(false);
            if (View.ChangeHousehold())
            {
                SetStartupView();
            }
            else
            {
                if (_activeChartData.Items.Any())
                {
                    View.ShowChart(true);
                }
            }
        }

        public void RefreshView()
        {
            if (AppGlobals.UnitTesting)
            {
                return;
            }
            //BudgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
            var context = AppGlobals.DataRepository.GetDataContext();
            var mainTable = context.GetTable<MainBudget>();
            context.RemoveRange(mainTable);
            if (context.Commit("Clearing Main Budget Table"))
            {
                var budgetItems = GetBudgetItems(context);

                context.AddRange(budgetItems);
                context.Commit("Adding Main Budgets");
            }
            BudgetLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));
            BankLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));

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

            MakeBudgetChartData();
            MakeActualChartData();

            if (_activeChartData.Items.Any())
            {
                View.ShowChart();
            }
        }

        private List<MainBudget> GetBudgetItems(IDbContext context)
        {
            var budgetItems = new List<MainBudget>();

            var periodHistoryTable = context.GetTable<BudgetPeriodHistory>();
            var periodHistory = periodHistoryTable
                .Include(p => p.BudgetItem)
                .Where(p => p.PeriodType == (byte)PeriodHistoryTypes.Monthly
                            && p.BudgetItem.Type != (byte)BudgetItemTypes.Transfer
                            && p.PeriodEndingDate.Month == CurrentMonthEnding.Month
                            && p.PeriodEndingDate.Year == CurrentMonthEnding.Year)
                .OrderBy(p => p.BudgetItem.Description);

            foreach (var budgetPeriodHistory in periodHistory)
            {
                var budgetItem = budgetItems
                    .FirstOrDefault(p => p.BudgetItemId == budgetPeriodHistory.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = new MainBudget();
                    budgetItem.BudgetItemId = budgetPeriodHistory.BudgetItemId;
                    budgetItem.ItemType = (byte)budgetPeriodHistory.BudgetItem.Type;
                    budgetItem.BudgetItem = budgetPeriodHistory.BudgetItem;
                    budgetItems.Add(budgetItem);
                }

                budgetItem.BudgetAmount += (double)budgetPeriodHistory.ProjectedAmount;
                budgetItem.ActualAmount += (double)budgetPeriodHistory.ActualAmount;
            }

            var registerTable = context.GetTable<BankAccountRegisterItem>();
            var monthlyRegisterItems = registerTable
                .Include(p => p.BudgetItem)
                .Where(p => p.BudgetItem != null
                            && p.BudgetItem.Type != (byte)BudgetItemTypes.Transfer
                            && p.ItemDate.Month == CurrentMonthEnding.Month
                            && p.ItemDate.Year == CurrentMonthEnding.Year)
                .OrderBy(p => p.BudgetItem.Description);
            foreach (var bankAccountRegisterItem in monthlyRegisterItems)
            {
                var budgetItem = budgetItems
                    .FirstOrDefault(p => p.BudgetItemId == bankAccountRegisterItem.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = new MainBudget();
                    budgetItem.BudgetItemId = bankAccountRegisterItem.BudgetItemId.GetValueOrDefault();
                    budgetItem.ItemType = (byte)bankAccountRegisterItem.BudgetItem.Type;
                    budgetItem.BudgetItem = bankAccountRegisterItem.BudgetItem;
                    budgetItems.Add(budgetItem);
                }

                budgetItem.BudgetAmount += Math.Abs((double)bankAccountRegisterItem.ProjectedAmount);
                budgetItem.ActualAmount += Math.Abs((double)bankAccountRegisterItem.ActualAmount.GetValueOrDefault()
                                                    + (double)bankAccountRegisterItem.ProjectedAmount);
            }

            return budgetItems;
        }

        private void MakeBudgetChartData()
        {
            _initialBudgetChartData = new ChartData();
            _initialBudgetChartData.Title = "Budget";

            var lookupDefinition = BudgetLookupDefinition.Clone();
            lookupDefinition.InitialSortColumnDefinition =
                lookupDefinition.GetColumnDefinition(p => p.BudgetAmount);

            _activeChartData = _initialBudgetChartData;

            MakeChartData();

            BudgetChartData = _initialBudgetChartData;
        }

        private void MakeActualChartData()
        {
            _initialActualChartData = new ChartData();
            _initialActualChartData.Title = "Actual";

            var lookupDefinition = BudgetLookupDefinition.Clone();
            lookupDefinition.InitialSortColumnDefinition =
                lookupDefinition.GetColumnDefinition(p => p.ActualAmount);

            _activeChartData = _initialActualChartData;

            MakeChartData();

            ActualChartData = _initialActualChartData;
        }


        private void MakeChartData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var budgetItems = GetBudgetItems(context);

            _budgetItems.Clear();
            MakeChartData(budgetItems);
            //lookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            //var lookupData = new LookupData<MainBudgetLookup, MainBudget>(lookupDefinition, this);

            //lookupData.LookupDataChanged -= LookupData_LookupDataChanged;
            //lookupData.LookupDataChanged += LookupData_LookupDataChanged;

            //lookupData.GetInitData();
            //lookupData.GetPrintData();

            //var stop = false;

            //while (!stop)
            //{
            //    switch (lookupData.ScrollPosition)
            //    {
            //        case LookupScrollPositions.Bottom:
            //        case LookupScrollPositions.Disabled:
            //            stop = true;
            //            break;
            //        default:
            //            lookupData.GotoNextPage();
            //            break;
            //    }
            //}
        }

        private void MakeChartData(List<MainBudget> budgets)
        {
            var sortedBudgets = budgets;
            if (_activeChartData == _initialBudgetChartData)
            {
                sortedBudgets = budgets.OrderByDescending(p => p.BudgetAmount).ToList();
            }
            else
            {
                sortedBudgets = budgets.OrderByDescending(p => p.ActualAmount).ToList();
            }
            foreach (var mainBudget in sortedBudgets)
            {
                var itemType = (BudgetItemTypes)mainBudget.ItemType;
                if (itemType == BudgetItemTypes.Expense)
                {
                    //if (!_budgetItems.Contains(mainLookup.BudgetId))
                    {
                        _budgetItems.Add(mainBudget.BudgetItemId);
                        if (_activeChartData == _initialBudgetChartData)
                        {
                            _activeChartData.Items.Add(new ChartDataItem
                            {
                                Name = mainBudget.BudgetItem.Description,
                                Value = (double) mainBudget.BudgetAmount
                            });
                        }
                        else if (_activeChartData == _initialActualChartData)
                        {
                            _activeChartData.Items.Add(new ChartDataItem
                            {
                                Name = mainBudget.BudgetItem.Description,
                                Value = (double) mainBudget.ActualAmount
                            });
                        }
                    }
                }
            }
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

        private void LaunchAdvancedFind()
        {
            View.LaunchAdvancedFind();
        }

        public void SetCurrentMonthEnding(DateTime value, bool refreshView = true)
        {
            CurrentMonthEnding = GetPeriodEndDate(value);

            if (refreshView)
                RefreshView();
        }

        private void CloseApp()
        {
            View.CloseApp();
        }

        private void SyncPhone()
        {
            var systemMaster = AppGlobals.DataRepository.GetSystemMaster();
            var loginInput = new Login
            {
                UserName = systemMaster.PhoneLogin,
                Password = systemMaster.PhonePassword,
                Guid = Guid.NewGuid().ToString()
            };
            var phoneLogin = View.ShowPhoneSync(loginInput);
            if (phoneLogin != null)
            {
                systemMaster.PhoneLogin = phoneLogin.UserName;
                systemMaster.PhonePassword = phoneLogin.Password;
                AppGlobals.DataRepository.SaveSystemMaster(systemMaster);
            }
        }

        //private List<BudgetData> GetPhoneBudgetData()
        //{

        //}

        private static DateTime GetPeriodEndDate(DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        public List<BudgetData> GetBudgetData(StatisticsType type, ITwoTierProcedure procedure)
        {
            var result = new List<BudgetData>();
            var currentMonthEnding = CurrentMonthEnding;
            var month = CurrentMonth;
            switch (type)
            {
                case StatisticsType.Current:
                    break;
                case StatisticsType.Previous:
                    month = CurrentMonth.AddMonths(-1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            _setCurrentDateText = false;
            CurrentMonthEnding = GetPeriodEndDate(month);
            //var budgetItems = new List<int>();

            //var lookupData = new LookupData<MainBudgetLookup, MainBudget>(budgetLookupDefinition, this);
            var context = AppGlobals.DataRepository.GetDataContext();
            var budgetItems = GetBudgetItems(context);
            var total = budgetItems.Count;
            procedure.UpdateBottomTier("Processing Budgets", total, 1);
            var procedureTotal = total;
            total = 0;

            foreach (var budgetItem in budgetItems)
            {
                var difference = budgetItem.BudgetAmount - budgetItem.ActualAmount;
                switch ((BudgetItemTypes)budgetItem.ItemType)
                {
                    case BudgetItemTypes.Income:
                        difference = budgetItem.ActualAmount - budgetItem.BudgetAmount;
                        break;
                }
                result.Add(new BudgetData
                {
                    BudgetItemId = budgetItem.BudgetItemId,
                    Description = budgetItem.BudgetItem.Description,
                    BudgetAmount = budgetItem.BudgetAmount,
                    ActualAmount = budgetItem.ActualAmount,
                    Difference = difference,
                    HistoryExists = AppGlobals.DataRepository.HistoryExists(budgetItem.BudgetItemId, month),
                    CurrentDate = month
                });
            }


            SetCurrentMonthEnding(GetPeriodEndDate(currentMonthEnding), type == StatisticsType.Previous);
            _setCurrentDateText = true;
            return result;
        }

        public List<BankData> GetBankData(ITwoTierProcedure procedure)
        {
            var result = new List<BankData>();

            var bankLookupDefinition = CreateBankLookupDefinition();
            var lookupData = bankLookupDefinition
                .TableDefinition
                .LookupDefinition
                .GetLookupDataMaui(bankLookupDefinition, false);
            var total = lookupData.GetRecordCount();
            procedure.UpdateBottomTier("Processing Banks", total, 1);
            var procedureTotal = total;
            total = 0;
            lookupData.PrintOutput += (sender, args) =>
            {
                total += PageSize;
                procedure.UpdateBottomTier("Processing Banks", procedureTotal, total);
                foreach (var primaryKey in args.Result)
                {
                    var bankAccount = AppGlobals.LookupContext.BankAccounts
                        .GetEntityFromPrimaryKeyValue(primaryKey);
                    bankAccount = AppGlobals.DataRepository.GetBankAccount(bankAccount.Id, false);

                    var bankData = result.FirstOrDefault(p => p.BankId == bankAccount.Id);
                    if (bankData == null)
                    {
                        var newBankData = new BankData
                        {
                            BankId = bankAccount.Id,
                            CurrentBalance = bankAccount.CurrentBalance,
                            Description = bankAccount.Description,
                            ProjectedLowestBalance = bankAccount.ProjectedLowestBalanceAmount,
                            ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate.GetValueOrDefault(),
                        };
                        var accountType = (int)bankAccount.AccountType;
                        newBankData.AccountType = (BankAccountTypes)accountType;
                        result.Add(newBankData);
                        
                    }
                }
            };

            lookupData.DoPrintOutput(10);

            return result;
        }

        public List<BudgetStatistics> GetBudgetStatistics()
        {
            var result = new List<BudgetStatistics>();
            var budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(CurrentMonth,
                GetPeriodEndDate(CurrentMonth.AddMonths(-1)), GetPeriodEndDate(CurrentMonth.AddMonths(1)));

            var currentStats = PopulateBudgetStats(budgetTotals, StatisticsType.Current, GetPeriodEndDate(CurrentMonth));
            result.Add(currentStats);

            budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(GetPeriodEndDate(CurrentMonth.AddMonths(-1)),
                GetPeriodEndDate(CurrentMonth.AddMonths(-1)), GetPeriodEndDate(CurrentMonth.AddMonths(1)));

            var pastStats = PopulateBudgetStats(budgetTotals, StatisticsType.Previous,
                GetPeriodEndDate(CurrentMonth.AddMonths(-1)));
            result.Add(pastStats);

            return result;
        }

        private static BudgetStatistics PopulateBudgetStats(BudgetTotals budgetTotals, StatisticsType type, DateTime budgetMonth)
        {
            var budgetStats = new BudgetStatistics();
            budgetStats.Type = type;
            budgetStats.MonthEnding = budgetMonth;
            budgetStats.BudgetIncome = budgetTotals.TotalProjectedMonthlyIncome;
            budgetStats.BudgetExpenses = budgetTotals.TotalProjectedMonthlyExpenses;
            budgetStats.ActualIncome = budgetTotals.TotalActualMonthlyIncome;
            budgetStats.ActualExpenses = budgetTotals.TotalActualMonthlyExpenses;
            budgetStats.YtdIncome = budgetTotals.YearToDateIncome;
            budgetStats.YtdExpenses = budgetTotals.YearToDateExpenses;
            return budgetStats;
        }

        private void ChangeDate()
        {
            var newDate = new DateTime(
                CurrentMonthEnding.Year
                , CurrentMonthEnding.Month
                , CurrentMonthEnding.Day);

            var changeDateData = new ChangeDateData();
            changeDateData.NewDate = newDate;
            View.GetChangeDate(changeDateData);
            if (changeDateData.DialogResult)
            {
                SetCurrentMonthEnding(changeDateData.NewDate);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
