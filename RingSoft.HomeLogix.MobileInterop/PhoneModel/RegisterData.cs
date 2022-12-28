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


    public class RegisterData
    {
        public int BankAccountId { get; set; }

        public DateTime ItemDate { get; set; }

        public string Description { get; set; }

        public decimal ProjectedAmount { get; set; }

        public decimal EndingBalance { get; set; }

        public bool IsNegative { get; set; }

        public bool Completed { get; set; }

        public TransactionTypes TransactionType { get; set; }

        public BankAccountRegisterItemTypes RegisterItemType { get; set; }

        public string TransactionTypeText { get; set; }
    }
}
