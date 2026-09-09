using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.Recipes
{
    public sealed class UpdateRecipeRequestDto : IValidatableObject
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot contain only whitespace.", new[] { nameof(Name) });
            }
        }
    }
}
