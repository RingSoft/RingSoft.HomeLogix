using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class QifMapId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QifMapId",
                table: "BankTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_QifMapId",
                table: "BankTransactions",
                column: "QifMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions",
                column: "QifMapId",
                principalTable: "QifMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_QifMaps_QifMapId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_QifMapId",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "QifMapId",
                table: "BankTransactions");
        }
    }
}
