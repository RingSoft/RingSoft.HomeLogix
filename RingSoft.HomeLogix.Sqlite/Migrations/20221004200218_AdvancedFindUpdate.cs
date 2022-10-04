using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AdvancedFindUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.AlterColumn<string>(
                name: "Table",
                table: "AdvancedFinds",
                type: "nvarchar",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvancedFinds",
                type: "nvarchar",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters");

            migrationBuilder.AlterColumn<string>(
                name: "Table",
                table: "AdvancedFinds",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvancedFinds",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindFilters",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
