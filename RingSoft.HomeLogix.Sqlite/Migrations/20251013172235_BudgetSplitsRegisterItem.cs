using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BudgetSplitsRegisterItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetItemId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "BankTransactionBankAccountId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankTransactionTransactionId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegisterItemId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionBudget_BankTransactionBankAccountId_BankTransactionTransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankTransactionBankAccountId", "BankTransactionTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionBudget_RegisterItemId",
                table: "BankTransactionBudget",
                column: "RegisterItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankAccountRegisterItems_RegisterItemId",
                table: "BankTransactionBudget",
                column: "RegisterItemId",
                principalTable: "BankAccountRegisterItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankTransactionBankAccountId_BankTransactionTransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankTransactionBankAccountId", "BankTransactionTransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankAccountRegisterItems_RegisterItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankTransactionBankAccountId_BankTransactionTransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactionBudget_BankTransactionBankAccountId_BankTransactionTransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactionBudget_RegisterItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropColumn(
                name: "BankTransactionBankAccountId",
                table: "BankTransactionBudget");

            migrationBuilder.DropColumn(
                name: "BankTransactionTransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.DropColumn(
                name: "RegisterItemId",
                table: "BankTransactionBudget");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetItemId",
                table: "BankTransactionBudget",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankId", "TransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" });
        }
    }
}
