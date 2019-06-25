using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerLogbook.Data.Migrations
{
    public partial class BusinessUnitLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "BusinessUnits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b32cc6d-2fc9-4808-a0a6-b3877bf9a381",
                column: "ConcurrencyStamp",
                value: "669ce113-10df-45fa-a41e-93ccc62231a4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93ad4deb-b9f7-4a98-9585-8b79963aee55",
                column: "ConcurrencyStamp",
                value: "2bb66bb6-a478-4bbf-81bf-e5c7a0430ad3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d525385f-0b2d-4db4-a874-a2bf1b27ae3f",
                column: "ConcurrencyStamp",
                value: "4abb4cae-c931-4812-99ab-368f06a5f6b9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e911f11-9213-4f2a-8a6e-3bf971927630", "AQAAAAEAACcQAAAAEAULcJ+8WV0H0BMYxXxFJpo585T4duoBin6pUB2BnZx8qAmHottXGBTILED7WyvpZQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "BusinessUnits");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b32cc6d-2fc9-4808-a0a6-b3877bf9a381",
                column: "ConcurrencyStamp",
                value: "ca0b3fa4-b04f-4fca-9861-b4c6b9369984");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93ad4deb-b9f7-4a98-9585-8b79963aee55",
                column: "ConcurrencyStamp",
                value: "ba8654c2-1117-489c-99b4-957b9f8d9048");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d525385f-0b2d-4db4-a874-a2bf1b27ae3f",
                column: "ConcurrencyStamp",
                value: "99d4b90d-4058-49a2-979f-66773991fd40");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b8866b3-8436-4712-a5df-c68e40d987e1", "AQAAAAEAACcQAAAAEObBDoBe8yePbKg7DFaJMf3EdcySgW9m1XFv3o7p5BO5JfZdtUhltTEQeGyqs4EdBg==" });
        }
    }
}
