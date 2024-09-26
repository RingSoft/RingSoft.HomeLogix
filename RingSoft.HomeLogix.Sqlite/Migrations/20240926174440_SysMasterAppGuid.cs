using Microsoft.EntityFrameworkCore.Migrations;
using RingSoft.HomeLogix.DataAccess;

#nullable disable

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class SysMasterAppGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppGuid",
                table: "SystemMaster",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            HomeLogixModelBuilder.MigrateAppGuid(migrationBuilder);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppGuid",
                table: "SystemMaster");
        }
    }
}
