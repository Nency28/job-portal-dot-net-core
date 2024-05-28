using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class city4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_city_CountryId",
                table: "city",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_city_country_CountryId",
                table: "city",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_city_country_CountryId",
                table: "city");

            migrationBuilder.DropIndex(
                name: "IX_city_CountryId",
                table: "city");
        }
    }
}
