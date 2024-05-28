using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class jobapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "jobapplication",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "jobapplication",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "jobapplication",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_jobapplication_JobId",
                table: "jobapplication",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_jobapplication_UserId",
                table: "jobapplication",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobapplication_job_JobId",
                table: "jobapplication",
                column: "JobId",
                principalTable: "job",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobapplication_usermodel_UserId",
                table: "jobapplication",
                column: "UserId",
                principalTable: "usermodel",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobapplication_job_JobId",
                table: "jobapplication");

            migrationBuilder.DropForeignKey(
                name: "FK_jobapplication_usermodel_UserId",
                table: "jobapplication");

            migrationBuilder.DropIndex(
                name: "IX_jobapplication_JobId",
                table: "jobapplication");

            migrationBuilder.DropIndex(
                name: "IX_jobapplication_UserId",
                table: "jobapplication");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "jobapplication");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "jobapplication",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "jobapplication",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
