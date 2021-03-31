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
                    CurrentBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    EscrowBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    ProjectedEndingBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectedLowestBalanceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProjectedLowestBalanceAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyBudgetDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyBudgetWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentMonthDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentMonthWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousMonthDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousMonthWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentYearDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentYearWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousYearDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousYearWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    EscrowToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    EscrowDayOfMonth = table.Column<int>(type: "integer", nullable: true),
                    LastGenerationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_BankAccounts_EscrowToBankAccountId",
                        column: x => x.EscrowToBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    StartingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoEscrow = table.Column<bool>(type: "bit", nullable: false),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    MonthlyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentMonthAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousMonthAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentYearAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PreviousYearAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    EscrowBalance = table.Column<decimal>(type: "numeric", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegisterGuid = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    ItemDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    ProjectedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    TransferRegisterGuid = table.Column<string>(type: "nvarchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_BankAccountId",
                table: "BankAccountRegisterItems",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_BudgetItemId",
                table: "BankAccountRegisterItems",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts",
                column: "EscrowToBankAccountId");

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
                name: "BankAccountRegisterItems");

            migrationBuilder.DropTable(
                name: "SystemMaster");

            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
