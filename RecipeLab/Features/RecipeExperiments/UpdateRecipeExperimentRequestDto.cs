using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.RecipeExperiments
{
    public sealed class UpdateRecipeExperimentRequestDto : IValidatableObject
    {
        [Required]
        [MaxLength(120)]
        public string PreparationMethod { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? VariationNotes { get; set; }

        [MaxLength(2000)]
        public string? ResultNotes { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(PreparationMethod) && string.IsNullOrWhiteSpace(PreparationMethod))
            {
                yield return new ValidationResult("Preparation method cannot contain only whitespace.", new[] { nameof(PreparationMethod) });
            }
        }
    }
}
