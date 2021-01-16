using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class FinalizedBudgetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recalculate",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "BudgetMonthDeposits",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BudgetMonthWithdrawals",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Recalculate",
                table: "BankAccounts");

            migrationBuilder.AddColumn<int>(
                name: "EscrowDayOfMonth",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EscrowToBankAccountId",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts",
                column: "EscrowToBankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts",
                column: "EscrowToBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "EscrowDayOfMonth",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.AddColumn<bool>(
                name: "Recalculate",
                table: "BudgetItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetMonthDeposits",
                table: "BankAccounts",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetMonthWithdrawals",
                table: "BankAccounts",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Recalculate",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
