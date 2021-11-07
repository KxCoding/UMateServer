using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class TimetableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timetable",
                columns: table => new
                {
                    TimetableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    CourseId = table.Column<string>(nullable: true),
                    CourseName = table.Column<string>(nullable: true),
                    RoomName = table.Column<string>(nullable: true),
                    ProfessorName = table.Column<string>(nullable: true),
                    CourseDay = table.Column<string>(nullable: true),
                    StartTime = table.Column<string>(nullable: true),
                    EndTime = table.Column<string>(nullable: true),
                    BackgroundColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetable", x => x.TimetableId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetable");
        }
    }
}
