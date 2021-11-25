using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UMateModel.Migrations
{
    public partial class AddPlaceReviewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaceReview",
                columns: table => new
                {
                    PlaceReviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    PlaceId = table.Column<int>(nullable: true),
                    Taste = table.Column<string>(nullable: false),
                    Service = table.Column<string>(nullable: false),
                    Mood = table.Column<string>(nullable: false),
                    Price = table.Column<string>(nullable: false),
                    Amount = table.Column<string>(nullable: false),
                    StarRating = table.Column<double>(nullable: false),
                    ReviewText = table.Column<string>(nullable: false),
                    RecommendationCount = table.Column<int>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceReview", x => x.PlaceReviewId);
                    table.ForeignKey(
                        name: "FK_PlaceReview_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceReview_PlaceId",
                table: "PlaceReview",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceReview");
        }
    }
}
