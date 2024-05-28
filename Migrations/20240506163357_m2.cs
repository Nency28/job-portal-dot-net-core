using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_company_Industry",
                table: "company",
                column: "Industry");

            migrationBuilder.AddForeignKey(
                name: "FK_company_industry_Industry",
                table: "company",
                column: "Industry",
                principalTable: "industry",
                principalColumn: "IndustryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_company_industry_Industry",
                table: "company");

            migrationBuilder.DropIndex(
                name: "IX_company_Industry",
                table: "company");
        }
    }
}
