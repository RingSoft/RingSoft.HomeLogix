using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class ConfigureAdvancedFind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SearchForValue",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RightParentheses",
                table: "AdvancedFindFilters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryTableName",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryFieldName",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Operand",
                table: "AdvancedFindFilters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte>(
                name: "LeftParentheses",
                table: "AdvancedFindFilters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Formula",
                table: "AdvancedFindFilters",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "EndLogic",
                table: "AdvancedFindFilters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayValue",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "CustomDate",
                table: "AdvancedFindFilters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "FilterId",
                table: "AdvancedFindFilters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryTableName",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<string>(
                name: "Formula",
                table: "AdvancedFindColumns",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "FieldDataType",
                table: "AdvancedFindColumns",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte>(
                name: "DecimalFormatType",
                table: "AdvancedFindColumns",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColumnId",
                table: "AdvancedFindColumns",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SearchForValue",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "RightParentheses",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryTableName",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryFieldName",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Operand",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<byte>(
                name: "LeftParentheses",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "Formula",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "EndLogic",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayValue",
                table: "AdvancedFindFilters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "CustomDate",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "FilterId",
                table: "AdvancedFindFilters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "AdvancedFindColumns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryTableName",
                table: "AdvancedFindColumns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Formula",
                table: "AdvancedFindColumns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "AdvancedFindColumns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "FieldDataType",
                table: "AdvancedFindColumns",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<byte>(
                name: "DecimalFormatType",
                table: "AdvancedFindColumns",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "AdvancedFindColumns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColumnId",
                table: "AdvancedFindColumns",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
