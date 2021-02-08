using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Migrations
{
    public partial class RemovefieldfromRecipetablechangeNutrionValuetablenametoNutritionValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_NutritionValue_NutrionValueId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "NutrionValueId",
                table: "Recipes",
                newName: "NutritionValueId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_NutrionValueId",
                table: "Recipes",
                newName: "IX_Recipes_NutritionValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_NutritionValue_NutritionValueId",
                table: "Recipes",
                column: "NutritionValueId",
                principalTable: "NutritionValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_NutritionValue_NutritionValueId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "NutritionValueId",
                table: "Recipes",
                newName: "NutrionValueId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_NutritionValueId",
                table: "Recipes",
                newName: "IX_Recipes_NutrionValueId");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_NutritionValue_NutrionValueId",
                table: "Recipes",
                column: "NutrionValueId",
                principalTable: "NutritionValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
