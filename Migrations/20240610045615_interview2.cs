using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class interview2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interview",
                columns: table => new
                {
                    InterviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interview", x => x.InterviewId);
                    table.ForeignKey(
                        name: "FK_interview_job_JobId",
                        column: x => x.JobId,
                        principalTable: "job",
                        principalColumn: "JobId");
                    table.ForeignKey(
                        name: "FK_interview_usermodel_UserId",
                        column: x => x.UserId,
                        principalTable: "usermodel",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_interview_JobId",
                table: "interview",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_interview_UserId",
                table: "interview",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interview");
        }
    }
}
