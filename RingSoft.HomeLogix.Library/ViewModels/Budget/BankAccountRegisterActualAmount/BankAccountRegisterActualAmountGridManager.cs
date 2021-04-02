using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum ActualAmountGridColumns
    {
        Date = BankAccountRegisterActualAmountGridManager.DateColumnId,
        Store = BankAccountRegisterActualAmountGridManager.StoreColumnId,
        Amount = BankAccountRegisterActualAmountGridManager.AmountColumnId
    }

    public class BankAccountRegisterActualAmountGridManager : DataEntryGridManager
    {
        public const int DateColumnId = 1;
        public const int StoreColumnId = 2;
        public const int AmountColumnId = 3;

        public BankAccountRegisterActualAmountViewModel ViewModel { get; }

        public BankAccountRegisterActualAmountGridManager(BankAccountRegisterActualAmountViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ActualAmountGridRow(this);
        }


        public decimal GetTotalAmount()
        {
            var result = (decimal) 0;
            var rows = Rows.OfType<ActualAmountGridRow>();
            foreach (var actualAmountGridRow in rows)
            {
                result += actualAmountGridRow.Amount;
            }

            return result;
        }

        public void LoadGrid(IEnumerable<BankAccountRegisterItemAmountDetail> entityList)
        {
            if (entityList == null)
                return;

            Grid?.SetBulkInsertMode();
            PreLoadGridFromEntity();
            foreach (var entity in entityList)
            {
                AddRowFromEntity(entity);
            }

            PostLoadGridFromEntity();
            Grid?.SetBulkInsertMode(false);
        }

        public void AddRowFromEntity(BankAccountRegisterItemAmountDetail entity)
        {
            var newRow = new ActualAmountGridRow(this);
            AddRow(newRow);
            newRow.LoadFromEntity(entity);
            Grid?.UpdateRow(newRow);
        }

        public void SaveData()
        {
            if (Grid == null)
                return;

            ViewModel.ActualAmountCellProps.RegisterGridRow.ActualAmountDetails.Clear();
            Grid.CommitCellEdit();
            var rowIndex = 0;
            var rows = Rows.OfType<ActualAmountGridRow>().OrderBy(o => o.Date);

            foreach (var actualAmountGridRow in rows)
            {
                if (!actualAmountGridRow.IsNew)
                {
                    var amountDetail = new BankAccountRegisterItemAmountDetail();
                    actualAmountGridRow.SaveToEntity(amountDetail, rowIndex);
                    ViewModel.ActualAmountCellProps.RegisterGridRow.ActualAmountDetails.Add(amountDetail);
                    rowIndex++;
                }
            }
        }
    }
}
