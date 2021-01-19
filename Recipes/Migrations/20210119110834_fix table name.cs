using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Migrations
{
    public partial class fixtablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_NutrionValue_NutrionValueId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "NutrionValue");

            migrationBuilder.CreateTable(
                name: "NutritionValue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true),
                    Proteins = table.Column<int>(type: "int", nullable: true),
                    Fats = table.Column<int>(type: "int", nullable: true),
                    Сarbohydrates = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionValue", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_NutritionValue_NutrionValueId",
                table: "Recipes",
                column: "NutrionValueId",
                principalTable: "NutritionValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_NutritionValue_NutrionValueId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "NutritionValue");

            migrationBuilder.CreateTable(
                name: "NutrionValue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fats = table.Column<int>(type: "int", nullable: true),
                    Proteins = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    Сarbohydrates = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrionValue", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_NutrionValue_NutrionValueId",
                table: "Recipes",
                column: "NutrionValueId",
                principalTable: "NutrionValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
