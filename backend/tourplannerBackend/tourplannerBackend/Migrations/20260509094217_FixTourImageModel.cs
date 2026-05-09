using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tourplannerBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixTourImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourLog",
                table: "TourImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourLog",
                table: "TourImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
