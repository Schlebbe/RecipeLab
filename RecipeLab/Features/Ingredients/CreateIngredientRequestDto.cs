using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.Ingredients
{
    public sealed class CreateIngredientRequestDto : IValidatableObject
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot contain only whitespace.", new[] { nameof(Name) });
            }
        }
    }
}
