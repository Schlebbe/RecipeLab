namespace RecipeLab.Features.Recipes
{
    public sealed class TopRatedIngredientDto
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ExperimentCount { get; set; }
    }
}
