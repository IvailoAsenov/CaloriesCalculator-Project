using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriesCalculator.Migrations
{
    /// <inheritdoc />
    public partial class DeмоDb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedFoods_Foods_FoodId",
                table: "SelectedFoods");

            migrationBuilder.DropIndex(
                name: "IX_SelectedFoods_FoodId",
                table: "SelectedFoods");

            migrationBuilder.AlterColumn<int>(
                name: "FoodId",
                table: "SelectedFoods",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FoodName",
                table: "SelectedFoods",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedFoods_FoodName",
                table: "SelectedFoods",
                column: "FoodName");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedFoods_Foods_FoodName",
                table: "SelectedFoods",
                column: "FoodName",
                principalTable: "Foods",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedFoods_Foods_FoodName",
                table: "SelectedFoods");

            migrationBuilder.DropIndex(
                name: "IX_SelectedFoods_FoodName",
                table: "SelectedFoods");

            migrationBuilder.DropColumn(
                name: "FoodName",
                table: "SelectedFoods");

            migrationBuilder.AlterColumn<string>(
                name: "FoodId",
                table: "SelectedFoods",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedFoods_FoodId",
                table: "SelectedFoods",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedFoods_Foods_FoodId",
                table: "SelectedFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
