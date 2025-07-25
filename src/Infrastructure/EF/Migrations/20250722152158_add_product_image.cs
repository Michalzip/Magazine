﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magazine.src.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class addproductimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultImage",
                table: "Products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultImage",
                table: "Products");
        }
    }
}
