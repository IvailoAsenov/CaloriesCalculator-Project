using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriesCalculator.Migrations
{
    /// <inheritdoc />
    public partial class DeмоDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedFoods_Foods_Foodid",
                table: "SelectedFoods");

            migrationBuilder.RenameColumn(
                name: "Foodid",
                table: "SelectedFoods",
                newName: "FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_SelectedFoods_Foodid",
                table: "SelectedFoods",
                newName: "IX_SelectedFoods_FoodId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedFoods_Foods_FoodId",
                table: "SelectedFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Categories_CategoryId1",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectedFoods_Foods_FoodId",
                table: "SelectedFoods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_CategoryId1",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "SelectedFoods",
                newName: "Foodid");

            migrationBuilder.RenameIndex(
                name: "IX_SelectedFoods_FoodId",
                table: "SelectedFoods",
                newName: "IX_SelectedFoods_Foodid");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedFoods_Foods_Foodid",
                table: "SelectedFoods",
                column: "Foodid",
                principalTable: "Foods",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
