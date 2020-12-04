using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class BankAccountAndBudgetItem : Migration
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
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RecurringPeriod = table.Column<int>(type: "integer", nullable: false),
                    RecurringType = table.Column<int>(type: "smallint", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DoEscrow = table.Column<bool>(type: "bit", nullable: true),
                    EscrowBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    SpendingType = table.Column<int>(type: "smallint", nullable: false),
                    SpendingDayOfWeek = table.Column<int>(type: "smallint", nullable: false),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextTransactionDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_EscrowBankAccountId",
                        column: x => x.EscrowBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BankAccountId",
                table: "BudgetItems",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_EscrowBankAccountId",
                table: "BudgetItems",
                column: "EscrowBankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
