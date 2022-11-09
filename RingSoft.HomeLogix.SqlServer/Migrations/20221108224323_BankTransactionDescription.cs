using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    public partial class BankTransactionDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankTransactionText",
                table: "BankTransactions",
                newName: "Description");

            migrationBuilder.AddColumn<bool>(
                name: "FromBank",
                table: "BankTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromBank",
                table: "BankTransactions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "BankTransactions",
                newName: "BankTransactionText");
        }
    }
}
