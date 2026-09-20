namespace RecipeLab.Features.Recipes
{
    public sealed class RecipeStatisticsResponseDto
    {
        public int RecipeCount { get; set; }
        public int IngredientCount { get; set; }
        public int ExperimentCount { get; set; }
        public double? AverageRating { get; set; }
        public IReadOnlyList<TopRatedRecipeDto> TopRatedRecipes { get; set; } = [];
    }
}
