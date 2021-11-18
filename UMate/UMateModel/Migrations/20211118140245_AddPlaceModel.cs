using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class AddPlaceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    PlaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    UniversityName = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    InstagramId = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    ThumbnailImageUrl = table.Column<string>(nullable: true),
                    PlaceImageUrl0 = table.Column<string>(nullable: true),
                    PlaceImageUrl1 = table.Column<string>(nullable: true),
                    PlaceImageUrl2 = table.Column<string>(nullable: true),
                    PlaceImageUrl3 = table.Column<string>(nullable: true),
                    PlaceImageUrl4 = table.Column<string>(nullable: true),
                    PlaceImageUrl5 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.PlaceId);
                });

            migrationBuilder.CreateTable(
                name: "PlaceBookmark",
                columns: table => new
                {
                    PlaceBookmarkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    PlaceId = table.Column<int>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceBookmark", x => x.PlaceBookmarkId);
                    table.ForeignKey(
                        name: "FK_PlaceBookmark_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceBookmark_PlaceId",
                table: "PlaceBookmark",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceBookmark");

            migrationBuilder.DropTable(
                name: "Place");
        }
    }
}
