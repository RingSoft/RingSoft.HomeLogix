using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AddedAmountToRegisterEscrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "BankAccountRegisterItemEscrows",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BankAccountRegisterItemEscrows");
        }
    }
}
