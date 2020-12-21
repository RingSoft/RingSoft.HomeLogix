using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RemovedIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "BudgetItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "BudgetItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
