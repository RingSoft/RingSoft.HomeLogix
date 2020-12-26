using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        public decimal? CurrentBalance { get; set; }

        public decimal? BudgetMonthDeposits { get; set; }

        public decimal? BudgetMonthWithdrawals { get; set; }

        public decimal? CurrentMonthDeposits { get; set; }

        public decimal? CurrentMonthWithdrawals { get; set; }

        public decimal? CurrentYearDeposits { get; set; }

        public decimal? CurrentYearWithdrawals { get; set; }

        public DateTime? LowestBalanceDate { get; set; }

        public decimal? LowestBalanceAmount { get; set; }

        public decimal? EscrowBalance { get; set; }

        public string Notes { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Recalculate { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<BudgetItem> BudgetTransferFromItems { get; set; }

        public virtual ICollection<BankAccountRegisterItem> RegisterItems { get; set; }

        public virtual ICollection<BankAccountRegisterItem> BankAccountTransferFromRegisterItems { get; set; }

        public BankAccount()
        {
            BudgetItems = new HashSet<BudgetItem>();
            BudgetTransferFromItems = new HashSet<BudgetItem>();
            RegisterItems = new HashSet<BankAccountRegisterItem>();
            BankAccountTransferFromRegisterItems = new HashSet<BankAccountRegisterItem>();
        }
    }
}
