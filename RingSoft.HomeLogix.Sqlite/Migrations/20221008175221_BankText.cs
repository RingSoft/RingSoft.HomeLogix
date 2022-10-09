using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class BankText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankText",
                table: "History",
                type: "nvarchar",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankText",
                table: "BankAccountRegisterItems",
                type: "nvarchar",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankText",
                table: "History");

            migrationBuilder.DropColumn(
                name: "BankText",
                table: "BankAccountRegisterItems");
        }
    }
}
