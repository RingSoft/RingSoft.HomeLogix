using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class NoBudgetInBankTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_BudgetId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactionBudget_BudgetItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "BudgetItemId",
                table: "BankTransactionBudget");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "BankTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetItemId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BudgetId",
                table: "BankTransactions",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionBudget_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id");
        }
    }
}
