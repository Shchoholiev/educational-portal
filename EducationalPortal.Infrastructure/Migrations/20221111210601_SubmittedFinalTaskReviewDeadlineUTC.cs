using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class SubmittedFinalTaskReviewDeadlineUTC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMaterial_Materials_MaterialId",
                table: "UserMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMaterial_Users_UserId",
                table: "UserMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMaterial",
                table: "UserMaterial");

            migrationBuilder.RenameTable(
                name: "UserMaterial",
                newName: "UsersMaterials");

            migrationBuilder.RenameIndex(
                name: "IX_UserMaterial_MaterialId",
                table: "UsersMaterials",
                newName: "IX_UsersMaterials_MaterialId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDeadlineUTC",
                table: "SubmittedFinalTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersMaterials",
                table: "UsersMaterials",
                columns: new[] { "UserId", "MaterialId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMaterials_Materials_MaterialId",
                table: "UsersMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMaterials_Users_UserId",
                table: "UsersMaterials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersMaterials_Materials_MaterialId",
                table: "UsersMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersMaterials_Users_UserId",
                table: "UsersMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersMaterials",
                table: "UsersMaterials");

            migrationBuilder.DropColumn(
                name: "ReviewDeadlineUTC",
                table: "SubmittedFinalTasks");

            migrationBuilder.RenameTable(
                name: "UsersMaterials",
                newName: "UserMaterial");

            migrationBuilder.RenameIndex(
                name: "IX_UsersMaterials_MaterialId",
                table: "UserMaterial",
                newName: "IX_UserMaterial_MaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMaterial",
                table: "UserMaterial",
                columns: new[] { "UserId", "MaterialId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMaterial_Materials_MaterialId",
                table: "UserMaterial",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMaterial_Users_UserId",
                table: "UserMaterial",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
