using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_usermodel_UserModel",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_UserModel",
                table: "job");

            migrationBuilder.DropColumn(
                name: "UserModel",
                table: "job");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserModel",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_job_UserModel",
                table: "job",
                column: "UserModel");

            migrationBuilder.AddForeignKey(
                name: "FK_job_usermodel_UserModel",
                table: "job",
                column: "UserModel",
                principalTable: "usermodel",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
