using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.MasterData.Migrations
{
    public partial class AddedIsDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Households",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Households");
        }
    }
}
