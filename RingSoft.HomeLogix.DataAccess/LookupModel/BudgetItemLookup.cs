namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class BudgetItemLookup
    {
        public string Description { get; set; }

        public string ItemType { get; set; }

        public byte ItemTypeId { get; set; }

        public int RecurringPeriod { get; set; }

        public string RecurringType { get; set; }

        public double Amount { get; set; }

        public double MonthlyAmount { get; set; }
    }
}
