namespace RecipeLab.Domain
{
    public class RecipeIngredient
    {
        public Guid RecipeId { get; set; }
        public Guid IngredientId { get; set; }

        public string? Quantity { get; set; }

        public Recipe Recipe { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}
