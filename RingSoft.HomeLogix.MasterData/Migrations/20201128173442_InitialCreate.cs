using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.MasterData.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar", maxLength: 250, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Households");
        }
    }
}
