using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    public class HomeLogixGridEditHostFactory : LookupGridEditHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == ActualAmountCellProps.ActualAmountControlHostId)
                return new DataEntryGridActualAmountHost(grid);

            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
