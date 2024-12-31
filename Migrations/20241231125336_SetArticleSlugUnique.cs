﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.NET.Migrations
{
    /// <inheritdoc />
    public partial class SetArticleSlugUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug",
                table: "Articles",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Slug",
                table: "Articles");
        }
    }
}
