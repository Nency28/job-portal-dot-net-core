using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_usermodel_userModelUserId",
                table: "job");

            migrationBuilder.RenameColumn(
                name: "userModelUserId",
                table: "job",
                newName: "UserModel");

            migrationBuilder.RenameIndex(
                name: "IX_job_userModelUserId",
                table: "job",
                newName: "IX_job_UserModel");

            migrationBuilder.AddForeignKey(
                name: "FK_job_usermodel_UserModel",
                table: "job",
                column: "UserModel",
                principalTable: "usermodel",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_usermodel_UserModel",
                table: "job");

            migrationBuilder.RenameColumn(
                name: "UserModel",
                table: "job",
                newName: "userModelUserId");

            migrationBuilder.RenameIndex(
                name: "IX_job_UserModel",
                table: "job",
                newName: "IX_job_userModelUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_job_usermodel_userModelUserId",
                table: "job",
                column: "userModelUserId",
                principalTable: "usermodel",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
