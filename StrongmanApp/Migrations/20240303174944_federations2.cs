using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrongmanApp.Migrations
{
    /// <inheritdoc />
    public partial class federations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
