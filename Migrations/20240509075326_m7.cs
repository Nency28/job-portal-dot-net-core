using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_userdata_Course",
                table: "userdata",
                column: "Course");

            migrationBuilder.AddForeignKey(
                name: "FK_userdata_course_Course",
                table: "userdata",
                column: "Course",
                principalTable: "course",
                principalColumn: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userdata_course_Course",
                table: "userdata");

            migrationBuilder.DropIndex(
                name: "IX_userdata_Course",
                table: "userdata");
        }
    }
}
