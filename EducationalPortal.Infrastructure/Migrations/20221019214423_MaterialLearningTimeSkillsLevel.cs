using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class MaterialLearningTimeSkillsLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LearningMinutes",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CoursesSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LearningMinutes",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "CoursesSkills");
        }
    }
}
