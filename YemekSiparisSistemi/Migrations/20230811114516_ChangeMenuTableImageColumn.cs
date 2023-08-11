using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekSiparisSistemi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMenuTableImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "menu");

            migrationBuilder.AddColumn<string>(
                name: "MenuImage",
                table: "menu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuImage",
                table: "menu");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "menu",
                type: "image",
                nullable: true);
        }
    }
}
