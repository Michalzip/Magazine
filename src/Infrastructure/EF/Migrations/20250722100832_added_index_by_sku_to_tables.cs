using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magazine.src.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class addedindexbyskutotables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                table: "Products",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_Sku",
                table: "Prices",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_Sku",
                table: "Inventories",
                column: "Sku");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Prices_Sku",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_Sku",
                table: "Inventories");
        }
    }
}
