namespace RecipeLab.Domain
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
    }
}
