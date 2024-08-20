using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsAssistantProject.Migrations
{
    /// <inheritdoc />
    public partial class Removednamefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Truck");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Truck",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
