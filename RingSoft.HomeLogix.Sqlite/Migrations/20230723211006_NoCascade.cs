using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class NoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory",
                column: "HistoryId",
                principalTable: "History",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItemSources_SourceId",
                table: "QifMaps",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QifMaps_BudgetItems_BudgetId",
                table: "QifMaps",
                column: "BudgetId",
                principalTable: "BudgetItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceHistory_History_HistoryId",
                table: "SourceHistory",
                column: "HistoryId",
                principalTable: "History",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
