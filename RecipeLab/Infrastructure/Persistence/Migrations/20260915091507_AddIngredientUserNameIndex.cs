using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeLab.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientUserNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UserId_Name",
                table: "Ingredients",
                columns: new[] { "UserId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UserId_Name",
                table: "Ingredients");
        }
    }
}
