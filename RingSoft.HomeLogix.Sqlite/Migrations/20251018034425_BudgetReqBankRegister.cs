using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class BudgetReqBankRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetItemId",
                table: "BankAccountRegisterItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetItemId",
                table: "BankAccountRegisterItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id");
        }
    }
}
