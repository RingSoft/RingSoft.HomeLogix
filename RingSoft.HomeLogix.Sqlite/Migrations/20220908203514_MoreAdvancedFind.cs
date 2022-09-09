using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class MoreAdvancedFind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedAlert",
                table: "AdvancedFinds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RefreshCondition",
                table: "AdvancedFinds",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RefreshRate",
                table: "AdvancedFinds",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefreshValue",
                table: "AdvancedFinds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YellowAlert",
                table: "AdvancedFinds",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedAlert",
                table: "AdvancedFinds");

            migrationBuilder.DropColumn(
                name: "RefreshCondition",
                table: "AdvancedFinds");

            migrationBuilder.DropColumn(
                name: "RefreshRate",
                table: "AdvancedFinds");

            migrationBuilder.DropColumn(
                name: "RefreshValue",
                table: "AdvancedFinds");

            migrationBuilder.DropColumn(
                name: "YellowAlert",
                table: "AdvancedFinds");
        }
    }
}
