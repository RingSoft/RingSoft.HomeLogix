using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    public partial class AddedBudgetItemFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastTransactionDate",
                table: "BudgetItems",
                newName: "LastCompletedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "BudgetItems",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndingDateType",
                table: "BudgetItems",
                type: "smallint",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "EndingDateType",
                table: "BudgetItems");

            migrationBuilder.RenameColumn(
                name: "LastCompletedDate",
                table: "BudgetItems",
                newName: "LastTransactionDate");
        }
    }
}
