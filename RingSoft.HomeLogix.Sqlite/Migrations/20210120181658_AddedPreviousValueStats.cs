using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AddedPreviousValueStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousMonthAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousYearAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousMonthDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousMonthWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousYearDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousYearWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousMonthAmount",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "PreviousYearAmount",
                table: "BudgetItems");

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
        }
    }
}
