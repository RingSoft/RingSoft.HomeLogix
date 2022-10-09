using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class SourceHistoryBankText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankText",
                table: "SourceHistory",
                type: "nvarchar",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankText",
                table: "BankAccountRegisterItemAmountDetails",
                type: "nvarchar",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankText",
                table: "SourceHistory");

            migrationBuilder.DropColumn(
                name: "BankText",
                table: "BankAccountRegisterItemAmountDetails");
        }
    }
}
