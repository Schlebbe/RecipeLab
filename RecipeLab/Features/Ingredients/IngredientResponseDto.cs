namespace RecipeLab.Features.Ingredients
{
    public sealed class IngredientResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
