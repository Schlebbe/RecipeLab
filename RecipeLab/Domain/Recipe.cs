namespace RecipeLab.Domain
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
        public ICollection<RecipeExperiment> Experiments { get; set; } = [];
    }
}
