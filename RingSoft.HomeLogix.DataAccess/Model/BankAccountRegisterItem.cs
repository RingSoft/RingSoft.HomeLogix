using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BankAccountRegisterItemTypes
    {
        BudgetItem = 0,
        Miscellaneous = 1,
        TransferToBankAccount = 2,
    }

    public class BankAccountRegisterItem
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string RegisterGuid { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public int ItemType { get; set; }

        [Required]
        public DateTime ItemDate { get; set; }

        public int? BudgetItemId { get; set; }

        [AllowNull]
        public virtual BudgetItem BudgetItem { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal ProjectedAmount { get; set; }

        public bool IsNegative { get; set; }

        public decimal? ActualAmount { get; set; }

        public string TransferRegisterGuid { get; set; }

        public bool Completed { get; set; }

        [MaxLength(250)]
        public string BankText { get; set; }

        public virtual ICollection<BankAccountRegisterItemAmountDetail> AmountDetails { get; set; }

        public BankAccountRegisterItem()
        {
            AmountDetails = new HashSet<BankAccountRegisterItemAmountDetail>();
        }
    }
}