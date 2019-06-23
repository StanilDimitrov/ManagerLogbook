using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerLogbook.Data.Migrations
{
    public partial class SeedDataCencorWords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CensoredWords_BusinessUnits_BusinessUnitId",
                table: "CensoredWords");

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "CensoredWords",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessUnitId",
                table: "CensoredWords",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d525385f-0b2d-4db4-a874-a2bf1b27ae3f", "99d4b90d-4058-49a2-979f-66773991fd40", "Moderator", "MODERATOR" },
                    { "93ad4deb-b9f7-4a98-9585-8b79963aee55", "ba8654c2-1117-489c-99b4-957b9f8d9048", "Admin", "ADMIN" },
                    { "6b32cc6d-2fc9-4808-a0a6-b3877bf9a381", "ca0b3fa4-b04f-4fca-9861-b4c6b9369984", "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BusinessUnitId", "ConcurrencyStamp", "CurrentLogbookId", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Picture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf", 0, null, "3b8866b3-8436-4712-a5df-c68e40d987e1", null, "admin@admin.bg", false, true, null, "ADMIN@ADMIN.BG", "ADMIN", "AQAAAAEAACcQAAAAEObBDoBe8yePbKg7DFaJMf3EdcySgW9m1XFv3o7p5BO5JfZdtUhltTEQeGyqs4EdBg==", null, false, null, "QSV7IPN3NQOB7US3NWWJQV2BOPWLAWQC", false, "Admin" });

            migrationBuilder.InsertData(
                table: "BusinessUnitCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Hotels" },
                    { 2, "Restaurants" }
                });

            migrationBuilder.InsertData(
                table: "CensoredWords",
                columns: new[] { "Id", "BusinessUnitId", "Word" },
                values: new object[,]
                {
                    { 4, null, "dick" },
                    { 1, null, "bastard" },
                    { 3, null, "cock" },
                    { 15, null, "scrotum" },
                    { 14, null, "son of a bitch" },
                    { 2, null, "ass" },
                    { 12, null, "shit" },
                    { 13, null, "nigga" },
                    { 10, null, "mother fucker" },
                    { 9, null, "Fuck off" },
                    { 8, null, "fuck" },
                    { 7, null, "bitch" },
                    { 6, null, "porn" },
                    { 5, null, "bull shit" },
                    { 11, null, "pussy" }
                });

            migrationBuilder.InsertData(
                table: "NoteCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "TODO" },
                    { 3, "Event" },
                    { 4, "Maintenance" },
                    { 5, "Supplying issue" },
                    { 1, "Task" }
                });

            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sofia" },
                    { 2, "Plovdiv" },
                    { 3, "Burgas" },
                    { 4, "Varna" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf", "93ad4deb-b9f7-4a98-9585-8b79963aee55" });

            migrationBuilder.InsertData(
                table: "BusinessUnits",
                columns: new[] { "Id", "Address", "BusinessUnitCategoryId", "Email", "Information", "Name", "PhoneNumber", "Picture", "TownId" },
                values: new object[,]
                {
                    { 1, "bul. Maria Luiza 42", 1, "grandhotel@abv.bg", null, "Grand Hotel Sofia", "0897654321", null, 1 },
                    { 4, "bul. G. Dimitrov 46", 1, "0897656361", null, "Hotel Palermo", "0897454324", null, 1 },
                    { 6, "Student City", 2, "sweet@dir.bg", null, "Sweet Sofia", "0897554325", null, 1 },
                    { 2, "bul. Vasil Levski 42", 1, "mariot@abv.bg", null, "Mariot", "0897354213", null, 2 },
                    { 5, "bul. Marica 43", 1, "grandhotel@abv.bg", null, "Grand Hotel Plovdiv", "0896654621", null, 2 },
                    { 3, "bul. Hristo Botev 32", 1, "imperial@mail.bg", null, "Imperial Hotel", "0897454324", null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Logbooks",
                columns: new[] { "Id", "BusinessUnitId", "Name", "Picture" },
                values: new object[] { 1, 5, "Bar and Dinner", null });

            migrationBuilder.InsertData(
                table: "Logbooks",
                columns: new[] { "Id", "BusinessUnitId", "Name", "Picture" },
                values: new object[] { 2, 5, "Sweet Valley", null });

            migrationBuilder.InsertData(
                table: "Logbooks",
                columns: new[] { "Id", "BusinessUnitId", "Name", "Picture" },
                values: new object[] { 3, 3, "Ambasador", null });

            migrationBuilder.AddForeignKey(
                name: "FK_CensoredWords_BusinessUnits_BusinessUnitId",
                table: "CensoredWords",
                column: "BusinessUnitId",
                principalTable: "BusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CensoredWords_BusinessUnits_BusinessUnitId",
                table: "CensoredWords");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b32cc6d-2fc9-4808-a0a6-b3877bf9a381");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d525385f-0b2d-4db4-a874-a2bf1b27ae3f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf", "93ad4deb-b9f7-4a98-9585-8b79963aee55" });

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CensoredWords",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Logbooks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Logbooks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Logbooks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "NoteCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93ad4deb-b9f7-4a98-9585-8b79963aee55");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf");

            migrationBuilder.DeleteData(
                table: "BusinessUnitCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BusinessUnitCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "CensoredWords",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessUnitId",
                table: "CensoredWords",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CensoredWords_BusinessUnits_BusinessUnitId",
                table: "CensoredWords",
                column: "BusinessUnitId",
                principalTable: "BusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
