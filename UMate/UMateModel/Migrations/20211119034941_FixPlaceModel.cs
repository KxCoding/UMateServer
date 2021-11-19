using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class FixPlaceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "portal",
                table: "University",
                newName: "Portal");

            migrationBuilder.RenameColumn(
                name: "map",
                table: "University",
                newName: "Map");

            migrationBuilder.RenameColumn(
                name: "library",
                table: "University",
                newName: "Library");

            migrationBuilder.RenameColumn(
                name: "homepage",
                table: "University",
                newName: "Homepage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Portal",
                table: "University",
                newName: "portal");

            migrationBuilder.RenameColumn(
                name: "Map",
                table: "University",
                newName: "map");

            migrationBuilder.RenameColumn(
                name: "Library",
                table: "University",
                newName: "library");

            migrationBuilder.RenameColumn(
                name: "Homepage",
                table: "University",
                newName: "homepage");
        }
    }
}
