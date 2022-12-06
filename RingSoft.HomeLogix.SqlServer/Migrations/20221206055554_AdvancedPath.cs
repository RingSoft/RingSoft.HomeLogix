using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    public partial class AdvancedPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AdvancedFindFilters",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AdvancedFindColumns",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "AdvancedFindFilters");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "AdvancedFindColumns");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
