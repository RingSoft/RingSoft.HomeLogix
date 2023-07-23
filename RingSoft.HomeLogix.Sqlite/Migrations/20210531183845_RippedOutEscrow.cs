using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RippedOutEscrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.DropTable(
                name: "BankAccountRegisterItemEscrows");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "DoEscrow",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "EscrowBalance",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "EscrowBalance",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "EscrowDayOfMonth",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "EscrowToBankAccountId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ApplyEscrow",
                table: "BankAccountRegisterItems");

            migrationBuilder.DropColumn(
                name: "IsEscrowFrom",
                table: "BankAccountRegisterItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoEscrow",
                table: "BudgetItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "EscrowBalance",
                table: "BudgetItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EscrowBalance",
                table: "BankAccounts",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EscrowDayOfMonth",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EscrowToBankAccountId",
                table: "BankAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ApplyEscrow",
                table: "BankAccountRegisterItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEscrowFrom",
                table: "BankAccountRegisterItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItemEscrows",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItemEscrows", x => new { x.RegisterId, x.BudgetItemId });
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemEscrows_BankAccountRegisterItems_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "BankAccountRegisterItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemEscrows_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts",
                column: "EscrowToBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItemEscrows_BudgetItemId",
                table: "BankAccountRegisterItemEscrows",
                column: "BudgetItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccounts_EscrowToBankAccountId",
                table: "BankAccounts",
                column: "EscrowToBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
