using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrongmanApp.Migrations
{
    /// <inheritdoc />
    public partial class federations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Federations_FederationId",
                table: "Competitions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "News",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Federations",
                table: "Competitions");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateModified",
                table: "News",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Federations_FederationId",
                table: "Competitions",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id");
        }
    }
}
