using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class RipOutBudgetPayCCBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayCCBalance",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "PayCCDay",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "PayCCType",
                table: "BankAccountRegisterItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PayCCBalance",
                table: "BudgetItems",
                type: "smallint",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "PayCCDay",
                table: "BudgetItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "PayCCType",
                table: "BankAccountRegisterItems",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
