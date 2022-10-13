using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class FinalTasksFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedReviewQuestions_SubmittedFinalTasks_SubmittedFinalTaskId",
                table: "SubmittedReviewQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedReviewQuestions_SubmittedFinalTasks_SubmittedFinalTaskId",
                table: "SubmittedReviewQuestions",
                column: "SubmittedFinalTaskId",
                principalTable: "SubmittedFinalTasks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedReviewQuestions_SubmittedFinalTasks_SubmittedFinalTaskId",
                table: "SubmittedReviewQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedReviewQuestions_SubmittedFinalTasks_SubmittedFinalTaskId",
                table: "SubmittedReviewQuestions",
                column: "SubmittedFinalTaskId",
                principalTable: "SubmittedFinalTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
