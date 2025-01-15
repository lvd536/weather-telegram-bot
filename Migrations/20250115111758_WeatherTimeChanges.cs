using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBotPractice.Migrations
{
    /// <inheritdoc />
    public partial class WeatherTimeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WeatherTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeatherTime",
                table: "Users");
        }
    }
}
