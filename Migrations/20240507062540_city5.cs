using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApplication.Migrations
{
    public partial class city5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_city_StateId",
                table: "city",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_city_state_StateId",
                table: "city",
                column: "StateId",
                principalTable: "state",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_city_state_StateId",
                table: "city");

            migrationBuilder.DropIndex(
                name: "IX_city_StateId",
                table: "city");
        }
    }
}
