using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerLogbook.Data.Migrations
{
    public partial class ChangeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BusinessUnits_BusinessUnitId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Logbooks_SubBusinessUnits_SubBusinessUnitId",
                table: "Logbooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Logbooks_AspNetUsers_UserId1",
                table: "Logbooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId1",
                table: "ManagerTasks");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "SubBusinessUnits");

            migrationBuilder.DropIndex(
                name: "IX_ManagerTasks_UserId1",
                table: "ManagerTasks");

            migrationBuilder.DropIndex(
                name: "IX_Logbooks_UserId1",
                table: "Logbooks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BusinessUnitId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ManagerTasks");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "BusinessUnits");

            migrationBuilder.DropColumn(
                name: "BusinessUnitId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "ManagerTasks",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Logbooks",
                newName: "Picture");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Logbooks",
                newName: "LogbookCategoryId");

            migrationBuilder.RenameColumn(
                name: "SubBusinessUnitId",
                table: "Logbooks",
                newName: "BusinessUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Logbooks_SubBusinessUnitId",
                table: "Logbooks",
                newName: "IX_Logbooks_BusinessUnitId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ManagerTasks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ManagerTasks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ManagerTasks",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LogbookId",
                table: "ManagerTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskCategoryId",
                table: "ManagerTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ManagerTasks",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "Logbooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Logbooks",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "BusinessUnits",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "BusinessUnits",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BrandName",
                table: "BusinessUnits",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BusinessUnits",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LogbookCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogbookCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OriginalDescription = table.Column<string>(maxLength: 500, nullable: false),
                    EditedDescription = table.Column<string>(maxLength: 500, nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LogbookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Logbooks_LogbookId",
                        column: x => x.LogbookId,
                        principalTable: "Logbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTasks_LogbookId",
                table: "ManagerTasks",
                column: "LogbookId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTasks_TaskCategoryId",
                table: "ManagerTasks",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTasks_UserId",
                table: "ManagerTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logbooks_LogbookCategoryId",
                table: "Logbooks",
                column: "LogbookCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_LogbookId",
                table: "Reviews",
                column: "LogbookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logbooks_BusinessUnits_BusinessUnitId",
                table: "Logbooks",
                column: "BusinessUnitId",
                principalTable: "BusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logbooks_LogbookCategories_LogbookCategoryId",
                table: "Logbooks",
                column: "LogbookCategoryId",
                principalTable: "LogbookCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_Logbooks_LogbookId",
                table: "ManagerTasks",
                column: "LogbookId",
                principalTable: "Logbooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_TaskCategories_TaskCategoryId",
                table: "ManagerTasks",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId",
                table: "ManagerTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logbooks_BusinessUnits_BusinessUnitId",
                table: "Logbooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Logbooks_LogbookCategories_LogbookCategoryId",
                table: "Logbooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_Logbooks_LogbookId",
                table: "ManagerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_TaskCategories_TaskCategoryId",
                table: "ManagerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId",
                table: "ManagerTasks");

            migrationBuilder.DropTable(
                name: "LogbookCategories");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropIndex(
                name: "IX_ManagerTasks_LogbookId",
                table: "ManagerTasks");

            migrationBuilder.DropIndex(
                name: "IX_ManagerTasks_TaskCategoryId",
                table: "ManagerTasks");

            migrationBuilder.DropIndex(
                name: "IX_ManagerTasks_UserId",
                table: "ManagerTasks");

            migrationBuilder.DropIndex(
                name: "IX_Logbooks_LogbookCategoryId",
                table: "Logbooks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ManagerTasks");

            migrationBuilder.DropColumn(
                name: "LogbookId",
                table: "ManagerTasks");

            migrationBuilder.DropColumn(
                name: "TaskCategoryId",
                table: "ManagerTasks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ManagerTasks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Logbooks");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ManagerTasks",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Logbooks",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "LogbookCategoryId",
                table: "Logbooks",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "BusinessUnitId",
                table: "Logbooks",
                newName: "SubBusinessUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Logbooks_BusinessUnitId",
                table: "Logbooks",
                newName: "IX_Logbooks_SubBusinessUnitId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ManagerTasks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "ManagerTasks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ManagerTasks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "Logbooks",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "BusinessUnits",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "BusinessUnits",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "BrandName",
                table: "BusinessUnits",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BusinessUnits",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "BusinessUnits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "BusinessUnitId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedComment = table.Column<string>(nullable: true),
                    OriginalComment = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubBusinessUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubBusinessUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTasks_UserId1",
                table: "ManagerTasks",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Logbooks_UserId1",
                table: "Logbooks",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BusinessUnitId",
                table: "AspNetUsers",
                column: "BusinessUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BusinessUnits_BusinessUnitId",
                table: "AspNetUsers",
                column: "BusinessUnitId",
                principalTable: "BusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Logbooks_SubBusinessUnits_SubBusinessUnitId",
                table: "Logbooks",
                column: "SubBusinessUnitId",
                principalTable: "SubBusinessUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logbooks_AspNetUsers_UserId1",
                table: "Logbooks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId1",
                table: "ManagerTasks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
