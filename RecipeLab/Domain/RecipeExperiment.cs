namespace RecipeLab.Domain
{
    public class RecipeExperiment
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string PreparationMethod { get; set; } = string.Empty;
        public string? VariationNotes { get; set; }
        public string? ResultNotes { get; set; }
        public int Rating { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        public Recipe Recipe { get; set; } = null!;
    }
}
