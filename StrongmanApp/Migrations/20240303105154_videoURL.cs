using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrongmanApp.Migrations
{
    /// <inheritdoc />
    public partial class videoURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoURL",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoURL",
                table: "News");
        }
    }
}
