using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class RegisterPayCCTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCCPayment",
                table: "BankAccountRegisterItems");

            migrationBuilder.AddColumn<byte>(
                name: "RegisterPayCCType",
                table: "BankAccountRegisterItems",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterPayCCType",
                table: "BankAccountRegisterItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsCCPayment",
                table: "BankAccountRegisterItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
