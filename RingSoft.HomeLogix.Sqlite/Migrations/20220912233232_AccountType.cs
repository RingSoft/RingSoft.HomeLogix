using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "BudgetItems",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text(1073741823)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "BankAccounts",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text(1073741823)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AccountType",
                table: "BankAccounts",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "BudgetItems",
                type: "text(1073741823)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "BankAccounts",
                type: "text(1073741823)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);
        }
    }
}
