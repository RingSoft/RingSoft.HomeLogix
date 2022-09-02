using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AdvancedFindFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayValue",
                table: "AdvancedFindFilters",
                newName: "FormulaDisplayValue");

            migrationBuilder.AlterColumn<int>(
                name: "SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<byte>(
                name: "FormulaDataType",
                table: "AdvancedFindFilters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormulaDataType",
                table: "AdvancedFindFilters");

            migrationBuilder.RenameColumn(
                name: "FormulaDisplayValue",
                table: "AdvancedFindFilters",
                newName: "DisplayValue");

            migrationBuilder.AlterColumn<int>(
                name: "SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
