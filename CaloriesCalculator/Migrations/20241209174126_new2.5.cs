using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriesCalculator.Migrations
{
    /// <inheritdoc />
    public partial class new25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaloriesPer100g = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "SelectedFoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaloriesPer100g = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    FoodName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedFoods_Foods_FoodName",
                        column: x => x.FoodName,
                        principalTable: "Foods",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedFoods_Foods_Name",
                        column: x => x.Name,
                        principalTable: "Foods",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedFoods_FoodName",
                table: "SelectedFoods",
                column: "FoodName");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedFoods_Name",
                table: "SelectedFoods",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedFoods");

            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
