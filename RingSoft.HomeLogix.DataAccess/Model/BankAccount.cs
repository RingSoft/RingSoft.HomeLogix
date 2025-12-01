using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BankAccountTypes
    {
        [Description("Checking")]
        Checking = 0,
        [Description("Savings")]
        Savings = 1,
        [Description("Credit Card")]
        CreditCard = 2
    }

    public enum BankCreditCardOptions
    {
        [Description("Carry Balance")]
        CarryBalance = 0,
        [Description("Pay Off Each Month")]
        PayOffEachMonth = 1,
    }

    public class BankAccount
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        [DefaultValue(0)]
        public byte AccountType { get; set; }

        [Required]
        [DefaultValue(0)]
        public double CurrentBalance { get; set; }

        public double ProjectedEndingBalance { get; set; }

        public DateTime? ProjectedLowestBalanceDate { get; set; }

        [Required]
        [DefaultValue(0)]
        public double ProjectedLowestBalanceAmount { get; set; }

        [Required]
        [DefaultValue(0)]
        public double MonthlyBudgetDeposits { get; set; }

        [Required]
        [DefaultValue(0)]
        public double MonthlyBudgetWithdrawals { get; set; }

        public DateTime LastGenerationDate { get; set; }

        public string Notes { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool ShowInGraph { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool PendingGeneration { get; set; }

        public int StatementDayOfMonth { get; set; }

        public byte? CreditCardOption { get; set; }

        [Required]
        [DefaultValue(0)]
        public double BankAccountIntrestRate { get; set; }

        public int? InterestBudgetId { get; set; }

        public virtual BudgetItem InterestBudgetItem { get; set; }

        public int? PayCCBalanceBudgetId { get; set; }

        public virtual BudgetItem PayCCBalanceBudgetItem { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<BudgetItem> BudgetTransferFromItems { get; set; }

        public virtual ICollection<BankAccountRegisterItem> RegisterItems { get; set; }
        
        public virtual ICollection<History> History { get; set; }

        public virtual ICollection<History> TransferToHistory { get; set; }

        public ICollection<BankAccountPeriodHistory> PeriodHistory { get; set; }

        public ICollection<BankTransaction> Transactions { get; set; }

        public BankAccount()
        {
            BudgetItems = new HashSet<BudgetItem>();
            BudgetTransferFromItems = new HashSet<BudgetItem>();
            RegisterItems = new HashSet<BankAccountRegisterItem>();
            History = new HashSet<History>();
            TransferToHistory = new HashSet<History>();
            PeriodHistory = new HashSet<BankAccountPeriodHistory>();
            Transactions = new HashSet<BankTransaction>();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? base.ToString() : Description;
        }
    }
}
