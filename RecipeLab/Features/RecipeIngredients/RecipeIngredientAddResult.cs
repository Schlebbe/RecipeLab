namespace RecipeLab.Features.RecipeIngredients
{
    public sealed class RecipeIngredientAddResult
    {
        public RecipeIngredientAddStatus Status { get; set; }
        public RecipeIngredientResponseDto? RecipeIngredient { get; set; }
    }
}
