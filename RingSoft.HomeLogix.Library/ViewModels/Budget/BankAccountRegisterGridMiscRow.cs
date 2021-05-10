using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridMiscRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.Miscellaneous;

        public BudgetItemTypes ItemType { get; set; }

        public BankAccountRegisterGridMiscRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }
    }
}
