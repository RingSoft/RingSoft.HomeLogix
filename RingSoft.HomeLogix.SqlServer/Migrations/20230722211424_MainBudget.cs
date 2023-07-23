using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class MainBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ProjectedAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<double>(
                name: "ActualAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<double>(
                name: "MonthlyAmount",
                table: "BudgetItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "BudgetItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<double>(
                name: "ProjectedAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateTable(
                name: "MainBudget",
                columns: table => new
                {
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<byte>(type: "tinyint", nullable: false),
                    BudgetAmount = table.Column<double>(type: "numeric(38,17)", nullable: false),
                    ActualAmount = table.Column<double>(type: "numeric(38,17)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainBudget", x => x.BudgetItemId);
                    table.ForeignKey(
                        name: "FK_MainBudget_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainBudget");

            migrationBuilder.AlterColumn<double>(
                name: "ProjectedAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<double>(
                name: "ActualAmount",
                table: "BudgetPeriodHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<double>(
                name: "MonthlyAmount",
                table: "BudgetItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "BudgetItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<double>(
                name: "ProjectedAmount",
                table: "BankAccountRegisterItems",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");
        }
    }
}
