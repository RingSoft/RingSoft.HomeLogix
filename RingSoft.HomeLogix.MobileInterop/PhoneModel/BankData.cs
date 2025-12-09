using System.ComponentModel;

public enum BankAccountTypes
{
    [Description("Checking")]
    Checking = 0,
    [Description("Savings")]
    Savings = 1,
    [Description("Credit Card")]
    CreditCard = 2
}


namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public class BankData
    {
        public int BankId { get; set; }
        
        public string Description { get; set; }

        public double CurrentBalance { get; set; }

        public double ProjectedLowestBalance { get; set; }

        public DateTime ProjectedLowestBalanceDate { get; set; }

        public BankAccountTypes AccountType { get; set; }

    }
}
