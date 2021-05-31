using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Migrations
{
    public partial class ChangeRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Recipes_RecipeId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_RecipeId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Step",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepNumber",
                table: "Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MainImageId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Step_ImageId",
                table: "Step",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_MainImageId",
                table: "Recipes",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Images_MainImageId",
                table: "Recipes",
                column: "MainImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Images_ImageId",
                table: "Step",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Images_MainImageId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Images_ImageId",
                table: "Step");

            migrationBuilder.DropIndex(
                name: "IX_Step_ImageId",
                table: "Step");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_MainImageId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "StepNumber",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "RecipeId",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_RecipeId",
                table: "Images",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Recipes_RecipeId",
                table: "Images",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
