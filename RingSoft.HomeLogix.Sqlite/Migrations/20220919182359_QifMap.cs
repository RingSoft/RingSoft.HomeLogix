using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class QifMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QifMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BankText = table.Column<string>(type: "nvarchar", nullable: true),
                    BudgetId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QifMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QifMaps_BudgetItems_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QifMaps_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QifMaps_BudgetId",
                table: "QifMaps",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_QifMaps_SourceId",
                table: "QifMaps",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QifMaps");
        }
    }
}
