using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BankIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_Description",
                table: "BankAccounts",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_Description",
                table: "BankAccountRegisterItems",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_ItemDate",
                table: "BankAccountRegisterItems",
                column: "ItemDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_Description",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccountRegisterItems_Description",
                table: "BankAccountRegisterItems");

            migrationBuilder.DropIndex(
                name: "IX_BankAccountRegisterItems_ItemDate",
                table: "BankAccountRegisterItems");
        }
    }
}
