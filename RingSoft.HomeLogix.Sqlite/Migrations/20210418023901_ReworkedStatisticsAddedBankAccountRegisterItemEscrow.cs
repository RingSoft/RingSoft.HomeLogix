using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class ReworkedStatisticsAddedBankAccountRegisterItemEscrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentYearAmount",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "PreviousMonthAmount",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "PreviousYearAmount",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "CurrentMonthDeposits",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CurrentMonthWithdrawals",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CurrentYearDeposits",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CurrentYearWithdrawals",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "PreviousMonthDeposits",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "PreviousMonthWithdrawals",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "PreviousYearDeposits",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "PreviousYearWithdrawals",
                table: "BankAccounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentMonthEnding",
                table: "BudgetItems",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItemEscrows",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItemEscrows", x => new { x.RegisterId, x.BudgetItemId });
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemEscrows_BankAccountRegisterItems_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "BankAccountRegisterItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemEscrows_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItemEscrows_BudgetItemId",
                table: "BankAccountRegisterItemEscrows",
                column: "BudgetItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountRegisterItemEscrows");

            migrationBuilder.DropColumn(
                name: "CurrentMonthEnding",
                table: "BudgetItems");

            migrationBuilder.AddColumn<double>(
                name: "CurrentYearAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousMonthAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousYearAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "CurrentMonthDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "CurrentMonthWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "CurrentYearDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "CurrentYearWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousMonthDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousMonthWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousYearDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "PreviousYearWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
