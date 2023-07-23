using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class CreatedHistoryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccountPeriodHistory",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<byte>(type: "smallint", nullable: false),
                    PeriodEndingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalDeposits = table.Column<double>(type: "numeric", nullable: false),
                    TotalWithdrawals = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountPeriodHistory", x => new { x.BankAccountId, x.PeriodType, x.PeriodEndingDate });
                    table.ForeignKey(
                        name: "FK_BankAccountPeriodHistory_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPeriodHistory",
                columns: table => new
                {
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<byte>(type: "smallint", nullable: false),
                    PeriodEndingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProjectedAmount = table.Column<double>(type: "numeric", nullable: false),
                    ActualAmount = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPeriodHistory", x => new { x.BudgetItemId, x.PeriodType, x.PeriodEndingDate });
                    table.ForeignKey(
                        name: "FK_BudgetPeriodHistory_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    ProjectedAmount = table.Column<double>(type: "numeric", nullable: false),
                    ActualAmount = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_History_BankAccounts_TransferToBankAccountId",
                        column: x => x.TransferToBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_History_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceHistory", x => new { x.HistoryId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_SourceHistory_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceHistory_History_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "History",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_History_BankAccountId",
                table: "History",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_History_BudgetItemId",
                table: "History",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_History_TransferToBankAccountId",
                table: "History",
                column: "TransferToBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceHistory_SourceId",
                table: "SourceHistory",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountPeriodHistory");

            migrationBuilder.DropTable(
                name: "BudgetPeriodHistory");

            migrationBuilder.DropTable(
                name: "SourceHistory");

            migrationBuilder.DropTable(
                name: "History");
        }
    }
}
