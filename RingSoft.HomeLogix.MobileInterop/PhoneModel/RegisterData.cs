namespace RingSoft.HomeLogix.MobileInterop.PhoneModel
{
    public enum BankAccountRegisterItemTypes
    {
        BudgetItem = 0,
        Miscellaneous = 1,
        TransferToBankAccount = 2,
    }

    public enum TransactionTypes
    {
        Deposit = 0,
        Withdrawal = 1
    }

    public enum MobileRegisterPayCCTypes
    {
        None = 0,
        FromBank = 1,
        ToCC = 2,
    }


    public class RegisterData
    {
        public BankAccountTypes AccountType { get; set; }

        public int BankAccountId { get; set; }

        public DateTime ItemDate { get; set; }

        public string Description { get; set; }

        public double ProjectedAmount { get; set; }

        public double ActualAmount { get; set; }

        public double EndingBalance { get; set; }

        public bool IsNegative { get; set; }

        public bool Completed { get; set; }

        public TransactionTypes TransactionType { get; set; }

        public BankAccountRegisterItemTypes RegisterItemType { get; set; }

        public string TransactionTypeText { get; set; }

        public MobileRegisterPayCCTypes RegisterPayCCType { get; set; } = MobileRegisterPayCCTypes.None;
    }
}
