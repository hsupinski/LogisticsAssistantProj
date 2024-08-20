using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistantProject.Migrations
{
    /// <inheritdoc />
    public partial class Dbrework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakDuration",
                table: "TruckInTransit");

            migrationBuilder.DropColumn(
                name: "MaxVelocity",
                table: "TruckInTransit");

            migrationBuilder.DropColumn(
                name: "MinutesUntilBreak",
                table: "TruckInTransit");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Transit",
                newName: "TruckId");

            migrationBuilder.AddColumn<int>(
                name: "BreakDuration",
                table: "Transit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxVelocity",
                table: "Transit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesUntilBreak",
                table: "Transit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakDuration",
                table: "Transit");

            migrationBuilder.DropColumn(
                name: "MaxVelocity",
                table: "Transit");

            migrationBuilder.DropColumn(
                name: "MinutesUntilBreak",
                table: "Transit");

            migrationBuilder.RenameColumn(
                name: "TruckId",
                table: "Transit",
                newName: "Distance");

            migrationBuilder.AddColumn<int>(
                name: "BreakDuration",
                table: "TruckInTransit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxVelocity",
                table: "TruckInTransit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesUntilBreak",
                table: "TruckInTransit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
