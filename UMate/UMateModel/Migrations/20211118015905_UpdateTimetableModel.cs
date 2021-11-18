using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class UpdateTimetableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "Timetable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "Timetable");
        }
    }
}
