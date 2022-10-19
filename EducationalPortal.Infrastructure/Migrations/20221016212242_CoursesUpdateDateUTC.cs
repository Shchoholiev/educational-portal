using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPortal.Infrastructure.Migrations
{
    public partial class CoursesUpdateDateUTC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateUTC",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDateUTC",
                table: "Courses");
        }
    }
}
