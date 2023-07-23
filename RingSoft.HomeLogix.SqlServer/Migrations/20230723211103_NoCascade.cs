using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class NoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "SourceHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BudgetAmount",
                table: "MainBudget",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "MainBudget",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "History",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "History",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyAmount",
                table: "BudgetItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentMonthAmount",
                table: "BudgetItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BudgetItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankTransactions",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankTransactionBudget",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedLowestBalanceAmount",
                table: "BankAccounts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedEndingBalance",
                table: "BankAccounts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyBudgetWithdrawals",
                table: "BankAccounts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyBudgetDeposits",
                table: "BankAccounts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentBalance",
                table: "BankAccounts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankAccountRegisterItemAmountDetails",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithdrawals",
                table: "BankAccountPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeposits",
                table: "BankAccountPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory",
                column: "HistoryId",
                principalTable: "History",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "SourceHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BudgetAmount",
                table: "MainBudget",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "MainBudget",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "History",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "History",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyAmount",
                table: "BudgetItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentMonthAmount",
                table: "BudgetItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BudgetItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankTransactions",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankTransactionBudget",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedLowestBalanceAmount",
                table: "BankAccounts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedEndingBalance",
                table: "BankAccounts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyBudgetWithdrawals",
                table: "BankAccounts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyBudgetDeposits",
                table: "BankAccounts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentBalance",
                table: "BankAccounts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectedAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BankAccountRegisterItemAmountDetails",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithdrawals",
                table: "BankAccountPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeposits",
                table: "BankAccountPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory",
                column: "HistoryId",
                principalTable: "History",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
