using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.DevLogix.Tests.QualityAssurance
{
    public class BudgetView : TestDbMaintenanceView, IBudgetItemView
    {
        public void SetViewType(bool isCC = false)
        {
            
        }

        public void ShowMonthlyStatsControls(bool show = true)
        {
            
        }

        public bool AddAdjustment(BudgetItem budgetItem)
        {
            return true;
        }

        public void HandleValFail(ValFailControls control)
        {
            
        }
    }
}
