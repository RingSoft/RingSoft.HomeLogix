using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportColumns
    {
        Date = 0,
        Description = 1,
        TransactionType = 2,
        RegisterItem = 3,
        RegisterDate = 4,
        Source = 5,
        Amount = 6,
        Map = 7
    }

    public class BudgetSplit
    {
        public AutoFillValue BudgetItem { get; set; }
        public double Amount { get; set; }
    }
    public class ImportTransactionGridRow : DataEntryGridRow
    {
        public const int DateColumnId = (int) ImportColumns.Date;
        public const int DescriptionColumnId = (int) ImportColumns.Description;
        public const int TransactionTypeColumnId = (int) ImportColumns.TransactionType;
        public const int RegisterItemColumnId = (int) ImportColumns.RegisterItem;
        public const int RegisterDateColumnId = (int) ImportColumns.RegisterDate;
        //public const int AddNewColumnId = (int)ImportColumns.AddRegItem;
        public const int SourceColumnId = (int) ImportColumns.Source;
        public const int AmountColumnId = (int) ImportColumns.Amount;
        public const int MapColumnId = (int) ImportColumns.Map;

        public DateTime Date { get; set; } = DateTime.Today;
        public string Description { get; set; }

        public TransactionTypes TransactionTypes
        {
            get => (Budget.TransactionTypes) TransactionTypeItem.NumericValue;
            set => TransactionTypeItem = TransactionTypeComboBoxControlSetup.GetItem((int) value);
        }
        public TextComboBoxControlSetup TransactionTypeComboBoxControlSetup { get; set; }
        public TextComboBoxItem TransactionTypeItem { get; set; }
        public AutoFillSetup RegisterItemAutoFillSetup { get; set; }
        public AutoFillValue RegisterItemAutoFillValue { get; set; }
        public DateTime? RegisterDate { get; set; }
        public AutoFillSetup SourceAutoFillSetup { get; set; }
        public AutoFillValue SourceAutoFillValue { get; set; }
        public double Amount { get; set; }
        public bool MapTransaction { get; set; }
        public List<BudgetSplit> BudgetItemSplits { get; set; }

        public new ImportTransactionsGridManager Manager { get; set; }

        public QifMap QifMap { get; set; }

        public bool FromBank { get; set; }

        private AutoFillValue _newRegisterAutoFillValue;

        public ImportTransactionGridRow(ImportTransactionsGridManager manager) : base(manager)
        {
            Manager = manager;
            BudgetItemSplits = new List<BudgetSplit>();
            TransactionTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            TransactionTypeComboBoxControlSetup.LoadFromEnum<TransactionTypes>();
            TransactionTypes = TransactionTypes.Withdrawal;
            RegisterItemAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankRegisterLookup.Clone());

            RegisterItemAutoFillSetup.LookupAdd += RegisterItemAutoFillSetup_LookupAdd;
            RegisterItemAutoFillSetup.LookupView += RegisterItemAutoFillSetup_LookupView;
            //RegisterItemAutoFillSetup.AllowLookupAdd = false;
            //RegisterItemAutoFillSetup.AllowLookupView = false;
            SourceAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankTransactions.GetFieldDefinition(p => p.SourceId));
        }

        private void RegisterItemAutoFillSetup_LookupView(object sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            e.Handled = true;
            var registerItem =
                AppGlobals.LookupContext
                    .BankAccountRegisterItems
                    .GetEntityFromPrimaryKeyValue(e.SelectedPrimaryKeyValue);

            registerItem = registerItem.FillOutProperties(true);
            var itemType = (BankAccountRegisterItemTypes)registerItem.ItemType;
            switch (itemType)
            {
                case BankAccountRegisterItemTypes.BudgetItem:
                    ShowBudgetRegister(e);
                    break;
                case BankAccountRegisterItemTypes.Miscellaneous:
                    ShowMiscRegister(e, registerItem);
                    break;
                case BankAccountRegisterItemTypes.TransferToBankAccount:
                    if (registerItem.IsTransferMisc)
                    {
                        ShowMiscRegister(e, registerItem);
                    }
                    else
                    {
                        ShowBudgetRegister(e);
                        
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ShowBudgetRegister(LookupAddViewArgs e)
        {
            SystemGlobals.TableRegistry.ShowEditAddOnTheFly(e.SelectedPrimaryKeyValue);
            e.CallBackToken.OnRefreshData();
        }

        private void ShowMiscRegister(LookupAddViewArgs e, BankAccountRegisterItem registerItem)
        {
            if (Manager
                .ViewModel
                .BankViewModel
                .BankAccountView
                .ShowBankAccountMiscWindow(registerItem, new ViewModelInput()))
            {
                e.CallBackToken.OnRefreshData();
            }
        }

        private void RegisterItemAutoFillSetup_LookupAdd(object sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            if (Manager.ViewModel.BankViewModel.ViewModelInput != null)
                Manager.ViewModel.BankViewModel.ViewModelInput.RefreshImportRow = this;
            var registerItem = Manager.ViewModel.BankViewModel.GetNewRegisterItem();
            if (registerItem.Id > 0)
            {
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                e.NewRecordPrimaryKeyValue = _newRegisterAutoFillValue.PrimaryKeyValue;
                e.CallBackToken.OnCloseLookupWindow();
                e.CallBackToken.RefreshMode = AutoFillRefreshModes.DbSelect;
                e.CallBackToken.OnRefreshData();
                RegisterItemAutoFillValue = _newRegisterAutoFillValue;
                RegisterDate = registerItem.ItemDate;
                Manager.Grid?.RefreshGridView();
                _newRegisterAutoFillValue = null;
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            }

            e.Handled = true;
            if (Manager.ViewModel.BankViewModel.ViewModelInput != null)
                Manager.ViewModel.BankViewModel.ViewModelInput.RefreshImportRow = null;
            _newRegisterAutoFillValue = null;
        }

        internal void RefreshMiscBeforeClose(BankAccountRegisterItem registerItem)
        {
            if (registerItem.Id > 0)
            {
                _newRegisterAutoFillValue = registerItem.GetAutoFillValue();
            }

        }

        public override string ToString()
        {
            return Description;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportColumns) columnId;

            switch (column)
            {
                case ImportColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup() {DateFormatType = DateFormatTypes.DateOnly}, Date);
                case ImportColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ImportColumns.TransactionType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int)TransactionTypes);
                case ImportColumns.RegisterItem:
                    RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
                    if (TransactionTypes == TransactionTypes.Withdrawal)
                    {
                        RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition
                            .AddFixedFilter(
                                AppGlobals.LookupContext.BankAccountRegisterItems.GetFieldDefinition(p =>
                                    p.ProjectedAmount),
                                Conditions.LessThan, 0);
                    }
                    if (TransactionTypes == TransactionTypes.Deposit)
                    {
                        RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition
                            .AddFixedFilter(
                                AppGlobals.LookupContext.BankAccountRegisterItems.GetFieldDefinition(p =>
                                    p.ProjectedAmount),
                                Conditions.GreaterThan, 0);
                    }

                    RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition
                        .AddFixedFilter(
                            AppGlobals.LookupContext.BankAccountRegisterItems.GetFieldDefinition(p =>
                                (int)p.BankAccountId),
                            Conditions.Equals, this.Manager.ViewModel.BankViewModel.Id);

                    return new DataEntryGridAutoFillCellProps(this, columnId, RegisterItemAutoFillSetup,
                        RegisterItemAutoFillValue);
                case ImportColumns.RegisterDate:
                    return new DataEntryGridDateCellProps(this, columnId
                    , new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateOnly,
                        AllowNullValue = true,
                    }, RegisterDate);
                //case ImportColumns.AddRegItem:
                //    return new DataEntryGridButtonCellProps(this, columnId);
                case ImportColumns.Source:
                    return new DataEntryGridAutoFillCellProps(this, columnId, SourceAutoFillSetup,
                        SourceAutoFillValue);
                case ImportColumns.Amount:
                    return new ActualAmountCellProps(this, columnId,
                        new DecimalEditControlSetup {FormatType = DecimalEditFormatTypes.Currency}, Amount);
                case ImportColumns.Map:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, MapTransaction);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ImportColumns)columnId;
            switch (column)
            {
                case ImportColumns.RegisterItem:
                    if (BudgetItemSplits.Any())
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.RegisterDate:
                    return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                //case ImportColumns.AddRegItem:
                //    return new DataEntryGridButtonCellStyle() { Content = "Add" };
                case ImportColumns.Source:
                case ImportColumns.Amount:
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.Description:
                    var state = DataEntryGridCellStates.Enabled;
                    if (FromBank)
                    {
                        state |= DataEntryGridCellStates.Disabled;
                    }
                    return new DataEntryGridControlCellStyle() {State = state};
                case ImportColumns.Map:
                    if (!FromBank)
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.Date:
                case ImportColumns.TransactionType:
                    if (FromBank)
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ImportColumns)value.ColumnId;

            switch (column)
            {
                case ImportColumns.Date:
                    var dateProps = value as DataEntryGridDateCellProps;
                    if (dateProps != null)
                    {
                        if (dateProps.Value != null) 
                            Date = dateProps.Value.Value;
                    }
                    break;
                case ImportColumns.Description:
                    var textProps = value as DataEntryGridTextCellProps;
                    if (textProps != null) Description = textProps.Text;
                    break;
                case ImportColumns.TransactionType:
                    var comboProps = value as DataEntryGridCustomControlCellProps;
                    TransactionTypes = (TransactionTypes) comboProps.SelectedItemId;
                    break;
                case ImportColumns.RegisterItem:
                    var registerAutoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (registerAutoFillCellProps != null && registerAutoFillCellProps.AutoFillValue.IsValid())
                    {
                        RegisterItemAutoFillValue = registerAutoFillCellProps.AutoFillValue;
                        if (RegisterItemAutoFillValue.IsValid())
                        {
                            var entity = RegisterItemAutoFillValue.GetEntity<BankAccountRegisterItem>();
                            if (entity != null)
                            {
                                entity = entity.FillOutProperties(new List<TableDefinitionBase>());
                                RegisterDate = entity.ItemDate;
                            }
                            Manager.SetMapRowsBudget(this);
                        }
                    }
                    else
                    {
                        RegisterItemAutoFillValue = null;
                    }
                    break;
                case ImportColumns.Source:
                    var autoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (autoFillCellProps != null)
                    {
                        SourceAutoFillValue = autoFillCellProps.AutoFillValue;
                        if (SourceAutoFillValue.IsValid())
                        {
                            Manager.SetMapRowsSource(this);
                        }
                    }
                    break;
                case ImportColumns.Amount:
                    var decimalProps = value as ActualAmountCellProps;
                    if (decimalProps != null)
                    {
                        if (decimalProps.Value != null) Amount = (double)decimalProps.Value.Value;
                    }
                    break;
                case ImportColumns.Map:
                    var checkBoxProps = value as DataEntryGridCheckBoxCellProps;
                    MapTransaction = checkBoxProps.Value;
                    break;
                //case ImportColumns.AddRegItem:
                //    var registerItem = Manager.ViewModel.BankViewModel.GetNewRegisterItem();
                //    if (registerItem.Id > 0)
                //    {
                //        RegisterItemAutoFillValue = registerItem.GetAutoFillValue();
                //        RegisterDate = registerItem.ItemDate;
                //    }
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }
    }
}
