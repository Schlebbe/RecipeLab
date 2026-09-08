namespace RecipeLab.Features.Recipes
{
    public sealed class RecipeResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}
