using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tourplannerBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddImageIdToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Tours",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Tours");
        }
    }
}
