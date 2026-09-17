namespace RecipeLab.Features.RecipeExperiments
{
    public sealed class RecipeExperimentResponseDto
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string PreparationMethod { get; set; } = string.Empty;
        public string? VariationNotes { get; set; }
        public string? ResultNotes { get; set; }
        public int Rating { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}
