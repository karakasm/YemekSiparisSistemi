using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekSiparisSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleForeignKeyToCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "company",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_RoleId",
                table: "company",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_company_role_RoleId",
                table: "company",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_company_role_RoleId",
                table: "company");

            migrationBuilder.DropIndex(
                name: "IX_company_RoleId",
                table: "company");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "company");
        }
    }
}
