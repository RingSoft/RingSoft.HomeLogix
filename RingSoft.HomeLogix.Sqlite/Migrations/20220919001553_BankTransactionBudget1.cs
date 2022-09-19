using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class BankTransactionBudget1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankTransactionBudget",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    RowId = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionBudget", x => new { x.BankId, x.TransactionId, x.RowId });
                    table.ForeignKey(
                        name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                        columns: x => new { x.BankId, x.TransactionId },
                        principalTable: "BankTransactions",
                        principalColumns: new[] { "BankAccountId", "TransactionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionBudget_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactionBudget");
        }
    }
}
