using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class interview3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_interview_job_JobId",
                table: "interview");

            migrationBuilder.DropForeignKey(
                name: "FK_interview_usermodel_UserId",
                table: "interview");

            migrationBuilder.DropIndex(
                name: "IX_interview_JobId",
                table: "interview");

            migrationBuilder.DropIndex(
                name: "IX_interview_UserId",
                table: "interview");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_interview_JobId",
                table: "interview",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_interview_UserId",
                table: "interview",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_interview_job_JobId",
                table: "interview",
                column: "JobId",
                principalTable: "job",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_interview_usermodel_UserId",
                table: "interview",
                column: "UserId",
                principalTable: "usermodel",
                principalColumn: "UserId");
        }
    }
}
