namespace RecipeLab.Features.Recipes
{
    public sealed class TopRatedRecipeDto
    {
        public Guid RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ExperimentCount { get; set; }
    }
}
