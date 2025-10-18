using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportBudgetsColumn
    {
        RegisterItem = 0,
        RegisterDate = 1,
        Amount = 2
    }
    public class ImportBankTransactionsBudgetsGridRow : DataEntryGridRow
    {
        public const int RegisterColumnId = (int)ImportBudgetsColumn.RegisterItem;
        public const int RegisterDateColumnId = (int)ImportBudgetsColumn.RegisterDate;
        public const int AmountColumnId = (int)ImportBudgetsColumn.Amount;

        public ImportBankTransactionsBudgetManager Manager { get; set; }
        public AutoFillSetup RegisterItemAutoFillSetup { get; set; }
        public AutoFillValue RegisterItemAutoFillValue { get; set; }
        public DateTime? RegisterDate { get; set; }
        public double BudgetAmount { get; set; }

        public ImportBankTransactionsBudgetsGridRow(ImportBankTransactionsBudgetManager manager) : base(manager)
        {
            Manager = manager;
            RegisterItemAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankRegisterLookup.Clone())
                {AddViewParameter = Manager.ViewModel.Row.Manager.ViewModel.BankViewModel.ViewModelInput};

            RegisterItemAutoFillSetup.LookupAdd += RegisterItemAutoFillSetup_LookupAdd;
            RegisterItemAutoFillSetup.LookupView += RegisterItemAutoFillSetup_LookupView;
        }

        private void RegisterItemAutoFillSetup_LookupView(object sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            e.FromLookupControl = true;
            e.CallBackToken.RefreshData += (o, args) =>
            {
                Manager.ViewModel.Row.Manager.ViewModel
                    .BankViewModel.RefreshFromDb();
            };
        }

        private void RegisterItemAutoFillSetup_LookupAdd(object sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            var bankViewModel = Manager.ViewModel.Row.Manager.ViewModel.BankViewModel;
            var registerItem = bankViewModel.GetNewRegisterItem();
            if (registerItem.Id > 0)
            {
                var newRegisterAutoFillValue = registerItem.GetAutoFillValue();
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                e.NewRecordPrimaryKeyValue = newRegisterAutoFillValue.PrimaryKeyValue;
                e.CallBackToken.OnCloseLookupWindow();
                e.CallBackToken.RefreshMode = AutoFillRefreshModes.DbSelect;
                e.CallBackToken.OnRefreshData();
                RegisterItemAutoFillValue = newRegisterAutoFillValue;
                RegisterDate = registerItem.ItemDate;
                Manager.Grid?.RefreshGridView();
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            }

            e.Handled = true;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportBudgetsColumn) columnId;

            switch (column)
            {
                case ImportBudgetsColumn.RegisterItem:
                    var tranType = Manager
                                       .ViewModel
                                       .Row.TransactionTypes;
                    RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
                    if (tranType == TransactionTypes.Withdrawal)
                    {
                        RegisterItemAutoFillSetup.LookupDefinition.FilterDefinition
                            .AddFixedFilter(
                                AppGlobals.LookupContext.BankAccountRegisterItems.GetFieldDefinition(p =>
                                    p.ProjectedAmount),
                                Conditions.LessThan, 0);
                    }
                    if (tranType == TransactionTypes.Deposit)
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
                            Conditions.Equals, this.Manager.ViewModel.Row.Manager.ViewModel.BankViewModel.Id);

                    return new DataEntryGridAutoFillCellProps(this, columnId, RegisterItemAutoFillSetup,
                        RegisterItemAutoFillValue);

                case ImportBudgetsColumn.RegisterDate:
                    return new DataEntryGridDateCellProps(this, columnId
                        , new DateEditControlSetup
                        {
                            DateFormatType = DateFormatTypes.DateOnly,
                            AllowNullValue = true,
                        }, RegisterDate);

                case ImportBudgetsColumn.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId,
                        new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Currency,
                        }, BudgetAmount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ImportBudgetsColumn)columnId;
            switch (column)
            {
                case ImportBudgetsColumn.RegisterDate:
                    return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                default:
                    return base.GetCellStyle(columnId);
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ImportBudgetsColumn)value.ColumnId;

            switch (column)
            {
                case ImportBudgetsColumn.RegisterItem:
                    var registerAutoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (registerAutoFillCellProps != null)// && registerAutoFillCellProps.AutoFillValue.IsValid())
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
                        }
                    }
                    //else
                    //{
                    //    RegisterItemAutoFillValue = null;
                    //}
                    break;
                case ImportBudgetsColumn.Amount:
                    var amountCellProps = value as DataEntryGridDecimalCellProps;
                    if (amountCellProps != null)
                    {
                        if (amountCellProps.Value != null) BudgetAmount = (double)amountCellProps.Value.Value;
                        Manager.SetLastRowAmount();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void Dispose()
        {
            BudgetAmount = 0;
            Manager.SetLastRowAmount();
            base.Dispose();
        }
    }
}
