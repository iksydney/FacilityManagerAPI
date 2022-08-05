using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.Services.Data.Migrations
{
    public partial class Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Complaints",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Complaints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Complaints",
                type: "text",
                nullable: true);
        }
    }
}
