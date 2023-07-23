using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        Source = BankAccountRegisterActualAmountGridManager.SourceColumnId,
        Amount = BankAccountRegisterActualAmountGridManager.AmountColumnId,
        BankText = BankAccountRegisterActualAmountGridManager.BankTextColumnId
    }

    public class BankAccountRegisterActualAmountGridManager : DataEntryGridManager
    {
        public const int DateColumnId = 1;
        public const int SourceColumnId = 2;
        public const int AmountColumnId = 3;
        public const int BankTextColumnId = 4;

        public BankAccountRegisterActualAmountViewModel ViewModel { get; }

        public BankAccountRegisterActualAmountGridManager(BankAccountRegisterActualAmountViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ActualAmountGridRow(this);
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
                ViewModel.CalculateTotals();

            base.OnRowsChanged(e);
        }

        public double GetTotalAmount()
        {
            var result = (double) 0;
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

        public List<BankAccountRegisterItemAmountDetail> SaveData()
        {
            if (Grid == null)
                return null;

            var result = new List<BankAccountRegisterItemAmountDetail>();
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
                    result.Add(amountDetail);
                    rowIndex++;
                }
            }

            return result;
        }
    }
}
