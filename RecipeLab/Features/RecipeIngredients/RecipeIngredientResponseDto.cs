namespace RecipeLab.Features.RecipeIngredients
{
    public sealed class RecipeIngredientResponseDto
    {
        public Guid RecipeId { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string? Quantity { get; set; }
    }
}
