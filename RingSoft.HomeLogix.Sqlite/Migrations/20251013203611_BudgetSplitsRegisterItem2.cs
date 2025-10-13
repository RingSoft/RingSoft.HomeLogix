using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BudgetSplitsRegisterItem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankId", "TransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankId", "TransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" });
        }
    }
}
