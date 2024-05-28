using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Education",
                table: "job");

            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "job");

            migrationBuilder.RenameColumn(
                name: "Employer",
                table: "job",
                newName: "userModelUserId");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "job",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Industry",
                table: "job",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuaId",
                table: "job",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qualification",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_job_CompanyId",
                table: "job",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_job_Industry",
                table: "job",
                column: "Industry");

            migrationBuilder.CreateIndex(
                name: "IX_job_Qualification",
                table: "job",
                column: "Qualification");

            migrationBuilder.CreateIndex(
                name: "IX_job_userModelUserId",
                table: "job",
                column: "userModelUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_job_company_CompanyId",
                table: "job",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_industry_Industry",
                table: "job",
                column: "Industry",
                principalTable: "industry",
                principalColumn: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_job_qualification_Qualification",
                table: "job",
                column: "Qualification",
                principalTable: "qualification",
                principalColumn: "QuaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_usermodel_userModelUserId",
                table: "job",
                column: "userModelUserId",
                principalTable: "usermodel",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_company_CompanyId",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_industry_Industry",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_qualification_Qualification",
                table: "job");

            migrationBuilder.DropForeignKey(
                name: "FK_job_usermodel_userModelUserId",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_CompanyId",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_Industry",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_Qualification",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_userModelUserId",
                table: "job");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "job");

            migrationBuilder.DropColumn(
                name: "QuaId",
                table: "job");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "job");

            migrationBuilder.RenameColumn(
                name: "userModelUserId",
                table: "job",
                newName: "Employer");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "job",
                newName: "Company");

            migrationBuilder.AlterColumn<int>(
                name: "Industry",
                table: "job",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "job",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "job",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
