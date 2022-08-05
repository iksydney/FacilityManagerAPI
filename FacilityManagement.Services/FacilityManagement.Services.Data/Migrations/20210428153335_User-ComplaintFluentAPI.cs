using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.Services.Data.Migrations
{
    public partial class UserComplaintFluentAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stack",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stack",
                table: "AspNetUsers");
        }
    }
}
