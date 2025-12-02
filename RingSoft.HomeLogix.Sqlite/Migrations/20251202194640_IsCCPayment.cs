using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class IsCCPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PayCCBalanceDay",
                table: "BankAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCCPayment",
                table: "BankAccountRegisterItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCCPayment",
                table: "BankAccountRegisterItems");

            migrationBuilder.AlterColumn<int>(
                name: "PayCCBalanceDay",
                table: "BankAccounts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
