using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magazine.src.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class reducepropertiesinentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DefaultImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InteralProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsVendor",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsWire",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "NetPrice",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "NetPriceAfterDiscount",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Inventories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Products",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultImage",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteralProductId",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVendor",
                table: "Products",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWire",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternalId",
                table: "Prices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "NetPrice",
                table: "Prices",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetPriceAfterDiscount",
                table: "Prices",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatRate",
                table: "Prices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Inventories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Inventories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
