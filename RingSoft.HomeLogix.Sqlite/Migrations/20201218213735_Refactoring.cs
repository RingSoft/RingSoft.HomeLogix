using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_BankAccounts_EscrowBankAccountId",
                table: "BudgetItems");

            migrationBuilder.RenameColumn(
                name: "EscrowBankAccountId",
                table: "BudgetItems",
                newName: "TransferToBankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItems_EscrowBankAccountId",
                table: "BudgetItems",
                newName: "IX_BudgetItems_TransferToBankAccountId");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyAmount",
                table: "BudgetItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_BankAccounts_TransferToBankAccountId",
                table: "BudgetItems",
                column: "TransferToBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_BankAccounts_TransferToBankAccountId",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "MonthlyAmount",
                table: "BudgetItems");

            migrationBuilder.RenameColumn(
                name: "TransferToBankAccountId",
                table: "BudgetItems",
                newName: "EscrowBankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItems_TransferToBankAccountId",
                table: "BudgetItems",
                newName: "IX_BudgetItems_EscrowBankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_BankAccounts_EscrowBankAccountId",
                table: "BudgetItems",
                column: "EscrowBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
