using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    BudgetMonthDeposits = table.Column<decimal>(type: "numeric", nullable: true),
                    BudgetMonthWithdrawals = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentMonthDeposits = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentMonthWithdrawals = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentYearDeposits = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentYearWithdrawals = table.Column<decimal>(type: "numeric", nullable: true),
                    LowestBalanceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LowestBalanceAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EscrowBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true),
                    Recalculate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemMaster",
                columns: table => new
                {
                    HouseholdName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMaster", x => x.HouseholdName);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RecurringPeriod = table.Column<int>(type: "integer", nullable: false),
                    RecurringType = table.Column<int>(type: "smallint", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoEscrow = table.Column<bool>(type: "bit", nullable: false),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    LastCompletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextTransactionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MonthlyAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentMonthAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentYearAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    EscrowBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    Recalculate = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_TransferToBankAccountId",
                        column: x => x.TransferToBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BankAccountId",
                table: "BudgetItems",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_TransferToBankAccountId",
                table: "BudgetItems",
                column: "TransferToBankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "SystemMaster");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
