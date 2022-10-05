﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.HomeLogix.Sqlite;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    [DbContext(typeof(HomeLogixDbContext))]
    [Migration("20221004215210_AdvancedFindUpdate2")]
    partial class AdvancedFindUpdate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<bool?>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("FromFormula")
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("RedAlert")
                        .HasColumnType("integer");

                    b.Property<byte?>("RefreshCondition")
                        .HasColumnType("smallint");

                    b.Property<byte?>("RefreshRate")
                        .HasColumnType("smallint");

                    b.Property<int?>("RefreshValue")
                        .HasColumnType("integer");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("YellowAlert")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AdvancedFinds");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("ColumnId")
                        .HasColumnType("integer");

                    b.Property<string>("Caption")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("DecimalFormatType")
                        .HasColumnType("smallint");

                    b.Property<byte>("FieldDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<double>("PercentWidth")
                        .HasColumnType("numeric");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("AdvancedFindId", "ColumnId");

                    b.ToTable("AdvancedFindColumns");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("FilterId")
                        .HasColumnType("integer");

                    b.Property<bool>("CustomDate")
                        .HasColumnType("bit");

                    b.Property<byte>("EndLogic")
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<byte>("FormulaDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FormulaDisplayValue")
                        .HasColumnType("nvarchar");

                    b.Property<byte>("LeftParentheses")
                        .HasColumnType("smallint");

                    b.Property<byte>("Operand")
                        .HasColumnType("smallint");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("RightParentheses")
                        .HasColumnType("smallint");

                    b.Property<int?>("SearchForAdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<string>("SearchForValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("AdvancedFindId", "FilterId");

                    b.HasIndex("SearchForAdvancedFindId");

                    b.ToTable("AdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<byte>("AccountType")
                        .HasColumnType("smallint");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime?>("LastCompletedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("LastGenerationDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("MonthlyBudgetDeposits")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MonthlyBudgetWithdrawals")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<decimal>("ProjectedEndingBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ProjectedLowestBalanceAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("ProjectedLowestBalanceDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("ShowInGraph")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountPeriodHistory", b =>
                {
                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<byte>("PeriodType")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("PeriodEndingDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("TotalDeposits")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalWithdrawals")
                        .HasColumnType("numeric");

                    b.HasKey("BankAccountId", "PeriodType", "PeriodEndingDate");

                    b.ToTable("BankAccountPeriodHistory");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<decimal?>("ActualAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<int?>("BudgetItemId")
                        .HasColumnType("integer");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<bool>("IsNegative")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ItemDate")
                        .HasColumnType("datetime");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.Property<double>("ProjectedAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("RegisterGuid")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TransferRegisterGuid")
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("BudgetItemId");

                    b.ToTable("BankAccountRegisterItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItemAmountDetail", b =>
                {
                    b.Property<int>("RegisterId")
                        .HasColumnType("integer");

                    b.Property<int>("DetailId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.HasKey("RegisterId", "DetailId");

                    b.HasIndex("SourceId");

                    b.ToTable("BankAccountRegisterItemAmountDetails");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankTransaction", b =>
                {
                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int?>("BankAccountRegisterItemAmountDetailDetailId")
                        .HasColumnType("integer");

                    b.Property<int?>("BankAccountRegisterItemAmountDetailRegisterId")
                        .HasColumnType("integer");

                    b.Property<string>("BankTransactionText")
                        .HasColumnType("nvarchar");

                    b.Property<int?>("BudgetId")
                        .HasColumnType("integer");

                    b.Property<bool>("MapTransaction")
                        .HasColumnType("bit");

                    b.Property<int?>("QifMapId")
                        .HasColumnType("integer");

                    b.Property<int?>("SourceId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.Property<byte>("TransactionType")
                        .HasColumnType("smallint");

                    b.HasKey("BankAccountId", "TransactionId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("QifMapId");

                    b.HasIndex("SourceId");

                    b.HasIndex("BankAccountRegisterItemAmountDetailRegisterId", "BankAccountRegisterItemAmountDetailDetailId");

                    b.ToTable("BankTransactions");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankTransactionBudget", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.Property<int>("RowId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("BudgetItemId")
                        .HasColumnType("integer");

                    b.HasKey("BankId", "TransactionId", "RowId");

                    b.HasIndex("BudgetItemId");

                    b.ToTable("BankTransactionBudget");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<double>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("CurrentMonthAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CurrentMonthEnding")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime?>("EndingDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("LastCompletedDate")
                        .HasColumnType("datetime");

                    b.Property<double>("MonthlyAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<int>("RecurringPeriod")
                        .HasColumnType("integer");

                    b.Property<int>("RecurringType")
                        .HasColumnType("smallint");

                    b.Property<DateTime?>("StartingDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("TransferToBankAccountId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("TransferToBankAccountId");

                    b.ToTable("BudgetItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<bool>("IsIncome")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("BudgetItemSources");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetPeriodHistory", b =>
                {
                    b.Property<int>("BudgetItemId")
                        .HasColumnType("integer");

                    b.Property<byte>("PeriodType")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("PeriodEndingDate")
                        .HasColumnType("datetime");

                    b.Property<double>("ActualAmount")
                        .HasColumnType("numeric");

                    b.Property<double>("ProjectedAmount")
                        .HasColumnType("numeric");

                    b.HasKey("BudgetItemId", "PeriodType", "PeriodEndingDate");

                    b.ToTable("BudgetPeriodHistory");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<decimal>("ActualAmount")
                        .HasColumnType("numeric");

                    b.Property<int?>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<int?>("BudgetItemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.Property<decimal>("ProjectedAmount")
                        .HasColumnType("numeric");

                    b.Property<int?>("TransferToBankAccountId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("BudgetItemId");

                    b.HasIndex("TransferToBankAccountId");

                    b.ToTable("History");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.QifMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("BankText")
                        .HasColumnType("nvarchar");

                    b.Property<int>("BudgetId")
                        .HasColumnType("integer");

                    b.Property<int?>("SourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("SourceId");

                    b.ToTable("QifMaps");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.SourceHistory", b =>
                {
                    b.Property<int>("HistoryId")
                        .HasColumnType("integer");

                    b.Property<int>("DetailId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.HasKey("HistoryId", "DetailId");

                    b.HasIndex("SourceId");

                    b.ToTable("SourceHistory");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.SystemMaster", b =>
                {
                    b.Property<string>("HouseholdName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("HouseholdName");

                    b.ToTable("SystemMaster");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Columns")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdvancedFind");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Filters")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "SearchForAdvancedFind")
                        .WithMany("SearchForAdvancedFindFilters")
                        .HasForeignKey("SearchForAdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AdvancedFind");

                    b.Navigation("SearchForAdvancedFind");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountPeriodHistory", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "BankAccount")
                        .WithMany("PeriodHistory")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "BankAccount")
                        .WithMany("RegisterItems")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany()
                        .HasForeignKey("BudgetItemId");

                    b.Navigation("BankAccount");

                    b.Navigation("BudgetItem");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItemAmountDetail", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", "RegisterItem")
                        .WithMany("AmountDetails")
                        .HasForeignKey("RegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", "Source")
                        .WithMany("AmountDetails")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RegisterItem");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankTransaction", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany("Transactions")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.QifMap", "QifMap")
                        .WithMany("Transactions")
                        .HasForeignKey("QifMapId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", "Source")
                        .WithMany("Transactions")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItemAmountDetail", null)
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountRegisterItemAmountDetailRegisterId", "BankAccountRegisterItemAmountDetailDetailId");

                    b.Navigation("BankAccount");

                    b.Navigation("BudgetItem");

                    b.Navigation("QifMap");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankTransactionBudget", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany("TransactionBudgets")
                        .HasForeignKey("BudgetItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankTransaction", "BankTransaction")
                        .WithMany("BudgetItems")
                        .HasForeignKey("BankId", "TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankTransaction");

                    b.Navigation("BudgetItem");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "BankAccount")
                        .WithMany("BudgetItems")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "TransferToBankAccount")
                        .WithMany("BudgetTransferFromItems")
                        .HasForeignKey("TransferToBankAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BankAccount");

                    b.Navigation("TransferToBankAccount");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetPeriodHistory", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany("PeriodHistory")
                        .HasForeignKey("BudgetItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BudgetItem");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.History", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "BankAccount")
                        .WithMany("History")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany("History")
                        .HasForeignKey("BudgetItemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "TransferToBankAccount")
                        .WithMany("TransferToHistory")
                        .HasForeignKey("TransferToBankAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BankAccount");

                    b.Navigation("BudgetItem");

                    b.Navigation("TransferToBankAccount");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.QifMap", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", "BudgetItem")
                        .WithMany("Maps")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", "Source")
                        .WithMany("Maps")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BudgetItem");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.SourceHistory", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.History", "HistoryItem")
                        .WithMany("Sources")
                        .HasForeignKey("HistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", "Source")
                        .WithMany("History")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("HistoryItem");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Navigation("Columns");

                    b.Navigation("Filters");

                    b.Navigation("SearchForAdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.Navigation("BudgetItems");

                    b.Navigation("BudgetTransferFromItems");

                    b.Navigation("History");

                    b.Navigation("PeriodHistory");

                    b.Navigation("RegisterItems");

                    b.Navigation("Transactions");

                    b.Navigation("TransferToHistory");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", b =>
                {
                    b.Navigation("AmountDetails");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItemAmountDetail", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankTransaction", b =>
                {
                    b.Navigation("BudgetItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.Navigation("History");

                    b.Navigation("Maps");

                    b.Navigation("PeriodHistory");

                    b.Navigation("TransactionBudgets");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", b =>
                {
                    b.Navigation("AmountDetails");

                    b.Navigation("History");

                    b.Navigation("Maps");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.History", b =>
                {
                    b.Navigation("Sources");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.QifMap", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
