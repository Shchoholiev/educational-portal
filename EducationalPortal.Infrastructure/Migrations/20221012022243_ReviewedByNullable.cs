using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class ReviewedByNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevievedById",
                table: "SubmittedFinalTasks");

            migrationBuilder.AddColumn<string>(
                name: "ReviewedById",
                table: "SubmittedFinalTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedFinalTasks_ReviewedById",
                table: "SubmittedFinalTasks",
                column: "ReviewedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedFinalTasks_Users_ReviewedById",
                table: "SubmittedFinalTasks",
                column: "ReviewedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedFinalTasks_Users_ReviewedById",
                table: "SubmittedFinalTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedFinalTasks_ReviewedById",
                table: "SubmittedFinalTasks");

            migrationBuilder.DropColumn(
                name: "ReviewedById",
                table: "SubmittedFinalTasks");

            migrationBuilder.AddColumn<string>(
                name: "RevievedById",
                table: "SubmittedFinalTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
