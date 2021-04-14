using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class RenamedStoreToBankAccountRegisterItemSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_Stores_StoreId",
                table: "BankAccountRegisterItemAmountDetails");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "BankAccountRegisterItemAmountDetails",
                newName: "SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountRegisterItemAmountDetails_StoreId",
                table: "BankAccountRegisterItemAmountDetails",
                newName: "IX_BankAccountRegisterItemAmountDetails_SourceId");

            migrationBuilder.CreateTable(
                name: "BudgetItemSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    IsIncome = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItemSources", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BudgetItemSources_SourceId",
                table: "BankAccountRegisterItemAmountDetails",
                column: "SourceId",
                principalTable: "BudgetItemSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_BudgetItemSources_SourceId",
                table: "BankAccountRegisterItemAmountDetails");

            migrationBuilder.DropTable(
                name: "BudgetItemSources");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "BankAccountRegisterItemAmountDetails",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountRegisterItemAmountDetails_SourceId",
                table: "BankAccountRegisterItemAmountDetails",
                newName: "IX_BankAccountRegisterItemAmountDetails_StoreId");

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountRegisterItemAmountDetails_Stores_StoreId",
                table: "BankAccountRegisterItemAmountDetails",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
