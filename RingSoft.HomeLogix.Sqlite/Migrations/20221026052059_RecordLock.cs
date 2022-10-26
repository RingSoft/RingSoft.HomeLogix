using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RecordLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BankAccounts_BankAccountId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BudgetItemSources_SourceId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.CreateTable(
                name: "RecordLocks",
                columns: table => new
                {
                    Table = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    LockDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLocks", x => new { x.Table, x.PrimaryKey });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankId", "TransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BankAccounts_BankAccountId",
                table: "BankTransactions",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BudgetItemSources_SourceId",
                table: "BankTransactions",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions",
                column: "QifMapId",
                principalTable: "QifMaps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BankAccounts_BankAccountId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BudgetItemSources_SourceId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.DropTable(
                name: "RecordLocks");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                table: "BankTransactionBudget",
                columns: new[] { "BankId", "TransactionId" },
                principalTable: "BankTransactions",
                principalColumns: new[] { "BankAccountId", "TransactionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BankAccounts_BankAccountId",
                table: "BankTransactions",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BudgetItems_BudgetId",
                table: "BankTransactions",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BudgetItemSources_SourceId",
                table: "BankTransactions",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions",
                column: "QifMapId",
                principalTable: "QifMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
