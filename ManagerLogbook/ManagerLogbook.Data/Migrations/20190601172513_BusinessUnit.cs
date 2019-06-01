using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerLogbook.Data.Migrations
{
    public partial class BusinessUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "BusinessUnits",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "BusinessUnits");
        }
    }
}
