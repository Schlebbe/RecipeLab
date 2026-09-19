using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.RecipeIngredients
{
    public sealed class UpdateRecipeIngredientRequestDto
    {
        [MaxLength(80)]
        public string? Quantity { get; set; }
    }
}
