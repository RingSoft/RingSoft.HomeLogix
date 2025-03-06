using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class PayCCDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PayCCBalance",
                table: "BudgetItems",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<byte>(
                name: "PayCCDay",
                table: "BudgetItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayCCDay",
                table: "BudgetItems");

            migrationBuilder.AlterColumn<bool>(
                name: "PayCCBalance",
                table: "BudgetItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "smallint");
        }
    }
}
