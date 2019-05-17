using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagerLogbook.Data.Migrations
{
    public partial class PostsTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Statuses_StatusId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_Problems_AspNetUsers_UserId1",
                table: "Problems");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problems",
                table: "Problems");

            migrationBuilder.RenameTable(
                name: "Problems",
                newName: "ManagerTasks");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_UserId1",
                table: "ManagerTasks",
                newName: "IX_ManagerTasks_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_StatusId",
                table: "ManagerTasks",
                newName: "IX_ManagerTasks_StatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManagerTasks",
                table: "ManagerTasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OriginalComment = table.Column<string>(nullable: true),
                    EditedComment = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_Statuses_StatusId",
                table: "ManagerTasks",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId1",
                table: "ManagerTasks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_Statuses_StatusId",
                table: "ManagerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerTasks_AspNetUsers_UserId1",
                table: "ManagerTasks");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManagerTasks",
                table: "ManagerTasks");

            migrationBuilder.RenameTable(
                name: "ManagerTasks",
                newName: "Problems");

            migrationBuilder.RenameIndex(
                name: "IX_ManagerTasks_UserId1",
                table: "Problems",
                newName: "IX_Problems_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_ManagerTasks_StatusId",
                table: "Problems",
                newName: "IX_Problems_StatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problems",
                table: "Problems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedDesctiption = table.Column<string>(nullable: true),
                    OriginalDescription = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Statuses_StatusId",
                table: "Problems",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_AspNetUsers_UserId1",
                table: "Problems",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
