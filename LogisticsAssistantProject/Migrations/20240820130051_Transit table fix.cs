using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistantProject.Migrations
{
    /// <inheritdoc />
    public partial class Transittablefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "Transit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Transit");
        }
    }
}
