using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class UpdatePlaceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityName",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "UniversityName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "Place",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    UniversityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    homepage = table.Column<string>(nullable: true),
                    portal = table.Column<string>(nullable: true),
                    library = table.Column<string>(nullable: true),
                    map = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.UniversityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Place_UniversityId",
                table: "Place",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_University_UniversityId",
                table: "Place",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_University_UniversityId",
                table: "Place");

            migrationBuilder.DropTable(
                name: "University");

            migrationBuilder.DropIndex(
                name: "IX_Place_UniversityId",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                table: "Place",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
