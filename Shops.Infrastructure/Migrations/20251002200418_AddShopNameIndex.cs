using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shops.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShopNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shops_Name",
                table: "Shops",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shops_Name",
                table: "Shops");
        }
    }
}
