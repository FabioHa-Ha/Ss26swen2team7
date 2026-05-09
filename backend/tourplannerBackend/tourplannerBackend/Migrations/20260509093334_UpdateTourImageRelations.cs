using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tourplannerBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourImageRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Tours");

            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "TourImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourImages_TourId",
                table: "TourImages",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourImages_Tours_TourId",
                table: "TourImages",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourImages_Tours_TourId",
                table: "TourImages");

            migrationBuilder.DropIndex(
                name: "IX_TourImages_TourId",
                table: "TourImages");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "TourImages");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Tours",
                type: "integer",
                nullable: true);
        }
    }
}
