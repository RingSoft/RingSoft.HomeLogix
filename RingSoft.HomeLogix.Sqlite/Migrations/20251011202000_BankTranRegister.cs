using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BankTranRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                table: "BankTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_RegisterId",
                table: "BankTransactions",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BankAccountRegisterItems_RegisterId",
                table: "BankTransactions",
                column: "RegisterId",
                principalTable: "BankAccountRegisterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BankAccountRegisterItems_RegisterId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_RegisterId",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "BankTransactions");
        }
    }
}
