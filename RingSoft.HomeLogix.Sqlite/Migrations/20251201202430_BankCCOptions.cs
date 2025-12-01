using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BankCCOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BankAccountIntrestRate",
                table: "BankAccounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte>(
                name: "CreditCardOption",
                table: "BankAccounts",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterestBudgetId",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayCCBalanceBudgetId",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatementDayOfMonth",
                table: "BankAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_InterestBudgetId",
                table: "BankAccounts",
                column: "InterestBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_PayCCBalanceBudgetId",
                table: "BankAccounts",
                column: "PayCCBalanceBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BudgetItems_InterestBudgetId",
                table: "BankAccounts",
                column: "InterestBudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BudgetItems_PayCCBalanceBudgetId",
                table: "BankAccounts",
                column: "PayCCBalanceBudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BudgetItems_InterestBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BudgetItems_PayCCBalanceBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_InterestBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_PayCCBalanceBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountIntrestRate",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CreditCardOption",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "InterestBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "PayCCBalanceBudgetId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "StatementDayOfMonth",
                table: "BankAccounts");
        }
    }
}
