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
    [Migration("20211007234754_AddCompletedField")]
    partial class AddCompletedField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("LastGenerationDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("MonthlyBudgetDeposits")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MonthlyBudgetWithdrawals")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

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
                        .HasColumnType("text(1073741823)");

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

                    b.Property<int>("BankAccountId")
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
                        .WithMany("RegisterItems")
                        .HasForeignKey("BudgetItemId")
                        .OnDelete(DeleteBehavior.Cascade);

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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

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

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.Navigation("BudgetItems");

                    b.Navigation("BudgetTransferFromItems");

                    b.Navigation("History");

                    b.Navigation("PeriodHistory");

                    b.Navigation("RegisterItems");

                    b.Navigation("TransferToHistory");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", b =>
                {
                    b.Navigation("AmountDetails");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.Navigation("History");

                    b.Navigation("PeriodHistory");

                    b.Navigation("RegisterItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItemSource", b =>
                {
                    b.Navigation("AmountDetails");

                    b.Navigation("History");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.History", b =>
                {
                    b.Navigation("Sources");
                });
#pragma warning restore 612, 618
        }
    }
}