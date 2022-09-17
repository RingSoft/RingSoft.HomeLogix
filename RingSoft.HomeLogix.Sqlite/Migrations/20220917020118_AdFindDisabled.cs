using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AdFindDisabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "AdvancedFinds",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "AdvancedFinds");
        }
    }
}
