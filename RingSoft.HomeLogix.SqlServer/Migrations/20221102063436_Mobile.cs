using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    public partial class Mobile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneLogin",
                table: "SystemMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhonePassword",
                table: "SystemMaster",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneLogin",
                table: "SystemMaster");

            migrationBuilder.DropColumn(
                name: "PhonePassword",
                table: "SystemMaster");
        }
    }
}
