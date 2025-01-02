using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.NET.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSomething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleEntityCategoryEntity_CategoryEntity_CategoriesID",
                table: "ArticleEntityCategoryEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryEntity",
                table: "CategoryEntity");

            migrationBuilder.RenameTable(
                name: "CategoryEntity",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleEntityCategoryEntity_Categories_CategoriesID",
                table: "ArticleEntityCategoryEntity",
                column: "CategoriesID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleEntityCategoryEntity_Categories_CategoriesID",
                table: "ArticleEntityCategoryEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "CategoryEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryEntity",
                table: "CategoryEntity",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleEntityCategoryEntity_CategoryEntity_CategoriesID",
                table: "ArticleEntityCategoryEntity",
                column: "CategoriesID",
                principalTable: "CategoryEntity",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
