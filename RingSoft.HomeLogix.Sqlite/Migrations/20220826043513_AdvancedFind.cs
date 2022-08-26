using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AdvancedFind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedFinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", nullable: true),
                    Table = table.Column<string>(type: "nvarchar", nullable: true),
                    FromFormula = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindColumns",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "INTEGER", nullable: false),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    FieldName = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryTableName = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "TEXT", nullable: true),
                    Caption = table.Column<string>(type: "TEXT", nullable: true),
                    PercentWidth = table.Column<double>(type: "REAL", nullable: false),
                    Formula = table.Column<string>(type: "TEXT", nullable: true),
                    FieldDataType = table.Column<byte>(type: "INTEGER", nullable: false),
                    DecimalFormatType = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindColumns", x => new { x.AdvancedFindId, x.ColumnId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindFilters",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    FilterId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeftParentheses = table.Column<byte>(type: "INTEGER", nullable: false),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    FieldName = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryTableName = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "TEXT", nullable: true),
                    Operand = table.Column<byte>(type: "INTEGER", nullable: false),
                    SearchForValue = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayValue = table.Column<string>(type: "TEXT", nullable: true),
                    Formula = table.Column<string>(type: "TEXT", nullable: true),
                    SearchForAdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    CustomDate = table.Column<bool>(type: "INTEGER", nullable: false),
                    RightParentheses = table.Column<byte>(type: "INTEGER", nullable: false),
                    EndLogic = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindFilters", x => new { x.AdvancedFindId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                        column: x => x.SearchForAdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedFindFilters_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedFindColumns");

            migrationBuilder.DropTable(
                name: "AdvancedFindFilters");

            migrationBuilder.DropTable(
                name: "AdvancedFinds");
        }
    }
}
