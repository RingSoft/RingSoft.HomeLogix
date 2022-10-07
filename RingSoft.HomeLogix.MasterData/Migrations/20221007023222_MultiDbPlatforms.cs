using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.MasterData.Migrations
{
    public partial class MultiDbPlatforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Households",
                type: "nvarchar",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Households",
                type: "nvarchar",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<byte>(
                name: "AuthenticationType",
                table: "Households",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Database",
                table: "Households",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Households",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Platform",
                table: "Households",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Server",
                table: "Households",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Households",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthenticationType",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "Database",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "Server",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Households");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Households",
                type: "nvarchar",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Households",
                type: "nvarchar",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
