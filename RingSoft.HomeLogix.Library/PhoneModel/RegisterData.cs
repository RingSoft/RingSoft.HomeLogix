using System;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
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
