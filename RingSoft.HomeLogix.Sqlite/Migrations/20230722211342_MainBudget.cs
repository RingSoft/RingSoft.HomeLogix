using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class MainBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainBudget",
                columns: table => new
                {
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<byte>(type: "smallint", nullable: false),
                    BudgetAmount = table.Column<double>(type: "numeric", nullable: false),
                    ActualAmount = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainBudget", x => x.BudgetItemId);
                    table.ForeignKey(
                        name: "FK_MainBudget_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainBudget");
        }
    }
}
