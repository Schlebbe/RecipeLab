using System.ComponentModel.DataAnnotations;

namespace RecipeLab.Features.Recipes
{
    public sealed class RecipeSearchRequestDto
    {
        [MaxLength(120)]
        public string? Search { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 50)]
        public int PageSize { get; set; } = 10;
    }
}
