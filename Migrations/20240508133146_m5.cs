using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class m5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usermodel_country_CountryId",
                table: "usermodel");

            migrationBuilder.DropIndex(
                name: "IX_usermodel_CountryId",
                table: "usermodel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_usermodel_CountryId",
                table: "usermodel",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_usermodel_country_CountryId",
                table: "usermodel",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
