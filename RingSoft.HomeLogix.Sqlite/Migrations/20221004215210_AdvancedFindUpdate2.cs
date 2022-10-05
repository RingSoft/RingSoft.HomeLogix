using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AdvancedFindUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
