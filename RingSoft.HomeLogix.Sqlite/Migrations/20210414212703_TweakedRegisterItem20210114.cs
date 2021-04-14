using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class TweakedRegisterItem20210114 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BankAccountRegisterItems_RegisterId",
                table: "BankAccountRegisterItemAmountDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BankAccountRegisterItems_RegisterId",
                table: "BankAccountRegisterItemAmountDetails",
                column: "RegisterId",
                principalTable: "BankAccountRegisterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BankAccountRegisterItems_RegisterId",
                table: "BankAccountRegisterItemAmountDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BankAccountRegisterItems_RegisterId",
                table: "BankAccountRegisterItemAmountDetails",
                column: "RegisterId",
                principalTable: "BankAccountRegisterItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
