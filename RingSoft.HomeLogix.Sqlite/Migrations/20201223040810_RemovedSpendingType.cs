using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RemovedSpendingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpendingDayOfWeek",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "SpendingType",
                table: "BudgetItems");

            migrationBuilder.RenameColumn(
                name: "SpendingAmount",
                table: "BudgetItems",
                newName: "MonthlyAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyAmount",
                table: "BudgetItems",
                newName: "SpendingAmount");

            migrationBuilder.AddColumn<int>(
                name: "SpendingDayOfWeek",
                table: "BudgetItems",
                type: "smallint",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpendingType",
                table: "BudgetItems",
                type: "smallint",
                nullable: false,
                defaultValue: 0);
        }
    }
}
