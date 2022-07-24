using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RemovedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AlterColumn<int>(
                name: "BankAccountId",
                table: "History",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems",
                column: "BudgetItemId",
                principalTable: "BudgetItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                table: "BankAccountRegisterItems");

            migrationBuilder.AlterColumn<int>(
                name: "BankAccountId",
                table: "History",
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
    }
}
