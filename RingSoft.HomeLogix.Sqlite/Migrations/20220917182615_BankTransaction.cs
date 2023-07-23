using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class BankTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BankTransactionText = table.Column<string>(type: "nvarchar", nullable: true),
                    BudgetId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<double>(type: "numeric", nullable: false),
                    BankAccountRegisterItemAmountDetailDetailId = table.Column<int>(type: "integer", nullable: true),
                    BankAccountRegisterItemAmountDetailRegisterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => new { x.BankAccountId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccountRegisterItemAmountDetails_BankAccountRegisterItemAmountDetailRegisterId_BankAccountRegisterItemAmountDetailDetailId",
                        columns: x => new { x.BankAccountRegisterItemAmountDetailRegisterId, x.BankAccountRegisterItemAmountDetailDetailId },
                        principalTable: "BankAccountRegisterItemAmountDetails",
                        principalColumns: new[] { "RegisterId", "DetailId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BudgetItems_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankAccountRegisterItemAmountDetailRegisterId_BankAccountRegisterItemAmountDetailDetailId",
                table: "BankTransactions",
                columns: new[] { "BankAccountRegisterItemAmountDetailRegisterId", "BankAccountRegisterItemAmountDetailDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BudgetId",
                table: "BankTransactions",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_SourceId",
                table: "BankTransactions",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactions");
        }
    }
}
