using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekSiparisSistemi.Migrations
{
    /// <inheritdoc />
    public partial class ChangingProductImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_image",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "ProductImagePath",
                table: "product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImagePath",
                table: "product");

            migrationBuilder.AddColumn<byte[]>(
                name: "product_image",
                table: "product",
                type: "image",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
