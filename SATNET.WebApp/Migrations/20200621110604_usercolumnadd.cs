using Microsoft.EntityFrameworkCore.Migrations;

namespace SATNET.WebApp.Migrations
{
    public partial class usercolumnadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PriceTierId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceTierId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
