using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.RecipeIngredients
{
    public sealed class AddRecipeIngredientRequestDto : IValidatableObject
    {
        public Guid IngredientId { get; set; }

        [MaxLength(80)]
        public string? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IngredientId == Guid.Empty)
            {
                yield return new ValidationResult("IngredientId is required.", new[] { nameof(IngredientId) });
            }
        }
    }
}
