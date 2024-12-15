﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriesCalculator.Migrations
{
    /// <inheritdoc />
    public partial class DeмоDb6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Categories_CategoryId1",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_CategoryId1",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Foods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId1",
                table: "Foods",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Categories_CategoryId1",
                table: "Foods",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
