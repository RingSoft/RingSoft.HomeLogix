using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class ImportExpiredGridManager : DataEntryGridManager
    {
        public ImportExpiredViewModel ViewModel { get; private set; }

        public List<BankAccountRegisterGridRow> ExpiredRows { get; private set; }

        public ImportExpiredGridManager(ImportExpiredViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            return new ImportExpiredGridRow(this);
        }

        public void LoadGrid(List<BankAccountRegisterGridRow> rows)
        {
            ExpiredRows = rows;
            foreach (var gridRow in rows)
            {
                var importExpiredRow = GetNewRow() as ImportExpiredGridRow;
                importExpiredRow.LoadGridRow(gridRow);
                AddRow(importExpiredRow);
            }
            Grid?.RefreshGridView();
        }

        public void UpdateList()
        {
            ExpiredRows.Clear();
            foreach (var importExpiredGridRow in Rows.OfType<ImportExpiredGridRow>())
            {
                if (importExpiredGridRow.RemoveRow)
                {
                    ExpiredRows.Add(importExpiredGridRow.ExpiredRow);
                }
                else if (importExpiredGridRow.ClearRow)
                {
                    importExpiredGridRow.ExpiredRow.ActualAmount = 0;
                    importExpiredGridRow.ExpiredRow.Completed = true;
                    var registerItem = new BankAccountRegisterItem();
                    importExpiredGridRow.ExpiredRow.SaveToEntity(registerItem, 0,
                        importExpiredGridRow.ExpiredRow.ActualAmountDetails.ToList());
                    AppGlobals.DataRepository.SaveRegisterItem(registerItem,
                        importExpiredGridRow.ExpiredRow.ActualAmountDetails);
                }
            }
        }
    }
}
