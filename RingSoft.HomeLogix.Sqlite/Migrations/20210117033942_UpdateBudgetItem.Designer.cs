﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Sqlite;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    [DbContext(typeof(HomeLogixDbContext))]
    [Migration("20210117033942_UpdateBudgetItem")]
    partial class UpdateBudgetItem
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

                    b.Property<decimal?>("CurrentBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CurrentMonthDeposits")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CurrentMonthWithdrawals")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CurrentYearDeposits")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CurrentYearWithdrawals")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<decimal?>("EscrowBalance")
                        .HasColumnType("numeric");

                    b.Property<int?>("EscrowDayOfMonth")
                        .HasColumnType("integer");

                    b.Property<int?>("EscrowToBankAccountId")
                        .HasColumnType("integer");

                    b.Property<decimal?>("LowestBalanceAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("LowestBalanceDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.HasKey("Id");

                    b.HasIndex("EscrowToBankAccountId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItem", b =>
                {
                    b.Property<string>("RegisterId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<int?>("BudgetItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.Property<BankAccountTransactionTypes>("TransactionType")
                        .HasColumnType("integer");

                    b.Property<int?>("TransferToBankAccountId")
                        .HasColumnType("integer");

                    b.HasKey("RegisterId");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("BudgetItemId");

                    b.HasIndex("TransferToBankAccountId");

                    b.ToTable("BankAccountRegisterItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("CurrentMonthAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("CurrentYearAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<bool>("DoEscrow")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EndingDate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("EscrowBalance")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("LastCompletedDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("MonthlyAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("NextTransactionDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.Property<int>("RecurringPeriod")
                        .HasColumnType("integer");

                    b.Property<int>("RecurringType")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("StartingDate")
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

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.SystemMaster", b =>
                {
                    b.Property<string>("HouseholdName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("HouseholdName");

                    b.ToTable("SystemMaster");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "EscrowToBankAccount")
                        .WithMany("EscrowFromBankAccounts")
                        .HasForeignKey("EscrowToBankAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("EscrowToBankAccount");
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

                    b.HasOne("RingSoft.HomeLogix.DataAccess.Model.BankAccount", "TransferToBankAccount")
                        .WithMany("BankAccountTransferFromRegisterItems")
                        .HasForeignKey("TransferToBankAccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BankAccount");

                    b.Navigation("BudgetItem");

                    b.Navigation("TransferToBankAccount");
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

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BankAccount", b =>
                {
                    b.Navigation("BankAccountTransferFromRegisterItems");

                    b.Navigation("BudgetItems");

                    b.Navigation("BudgetTransferFromItems");

                    b.Navigation("EscrowFromBankAccounts");

                    b.Navigation("RegisterItems");
                });

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.BudgetItem", b =>
                {
                    b.Navigation("RegisterItems");
                });
#pragma warning restore 612, 618
        }
    }
}
