using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
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
        public decimal CurrentBalance { get; set; }

        public decimal? EscrowBalance { get; set; }

        public decimal ProjectedEndingBalance { get; set; }

        public DateTime? ProjectedLowestBalanceDate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal ProjectedLowestBalanceAmount { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal MonthlyBudgetDeposits { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal MonthlyBudgetWithdrawals { get; set; }

        public int? EscrowToBankAccountId { get; set; }

        public virtual BankAccount EscrowToBankAccount { get; set; }

        public int? EscrowDayOfMonth { get; set; }

        [Required]
        public DateTime LastGenerationDate { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<BudgetItem> BudgetTransferFromItems { get; set; }

        public virtual ICollection<BankAccountRegisterItem> RegisterItems { get; set; }
        
        public virtual ICollection<BankAccount> EscrowFromBankAccounts { get; set; }

        public BankAccount()
        {
            BudgetItems = new HashSet<BudgetItem>();
            BudgetTransferFromItems = new HashSet<BudgetItem>();
            RegisterItems = new HashSet<BankAccountRegisterItem>();
            EscrowFromBankAccounts = new HashSet<BankAccount>();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? base.ToString() : Description;
        }
    }
}
