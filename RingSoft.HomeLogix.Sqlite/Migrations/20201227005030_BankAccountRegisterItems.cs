using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class BankAccountRegisterItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItems",
                columns: table => new
                {
                    RegisterId = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    TransactionType = table.Column<long>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItems", x => x.RegisterId);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BankAccounts_TransferToBankAccountId",
                        column: x => x.TransferToBankAccountId,
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
                name: "IX_BankAccountRegisterItems_TransferToBankAccountId",
                table: "BankAccountRegisterItems",
                column: "TransferToBankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountRegisterItems");
        }
    }
}
