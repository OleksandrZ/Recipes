using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Migrations
{
    public partial class ChangeLetter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Сarbohydrates",
                table: "NutritionValue",
                newName: "Carbohydrates");

            migrationBuilder.CreateTable(
                name: "RecipeLikes",
                columns: table => new
                {
                    LikedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikedRecipeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeLikes", x => new { x.LikedByUserId, x.LikedRecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeLikes_AspNetUsers_LikedByUserId",
                        column: x => x.LikedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeLikes_Recipes_LikedRecipeId",
                        column: x => x.LikedRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeLikes_LikedRecipeId",
                table: "RecipeLikes",
                column: "LikedRecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeLikes");

            migrationBuilder.RenameColumn(
                name: "Carbohydrates",
                table: "NutritionValue",
                newName: "Сarbohydrates");
        }
    }
}
