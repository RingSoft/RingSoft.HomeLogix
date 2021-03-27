using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RedidTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItems_BankAccounts_TransferToBankAccountId",
                table: "BankAccountRegisterItems");

            migrationBuilder.DropIndex(
                name: "IX_BankAccountRegisterItems_TransferToBankAccountId",
                table: "BankAccountRegisterItems");

            migrationBuilder.DropColumn(
                name: "TransferToBankAccountId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AddColumn<string>(
                name: "TransferRegisterId",
                table: "BankAccountRegisterItems",
                type: "nvarchar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferRegisterId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AddColumn<int>(
                name: "TransferToBankAccountId",
                table: "BankAccountRegisterItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_TransferToBankAccountId",
                table: "BankAccountRegisterItems",
                column: "TransferToBankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItems_BankAccounts_TransferToBankAccountId",
                table: "BankAccountRegisterItems",
                column: "TransferToBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
