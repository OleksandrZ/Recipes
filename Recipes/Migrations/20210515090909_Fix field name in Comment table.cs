using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Migrations
{
    public partial class FixfieldnameinCommenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Comments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Comments",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Comments",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Comments",
                newName: "CreateAt");
        }
    }
}
