using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class UserMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialsBaseUser");

            migrationBuilder.CreateTable(
                name: "UserMaterial",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    LearnDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMaterial", x => new { x.UserId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_UserMaterial_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMaterial_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMaterial_MaterialId",
                table: "UserMaterial",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMaterial");

            migrationBuilder.CreateTable(
                name: "MaterialsBaseUser",
                columns: table => new
                {
                    MaterialsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsBaseUser", x => new { x.MaterialsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MaterialsBaseUser_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialsBaseUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsBaseUser_UsersId",
                table: "MaterialsBaseUser",
                column: "UsersId");
        }
    }
}
