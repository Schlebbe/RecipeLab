namespace RecipeLab.Features.Recipes
{
    public sealed class RecipeSearchResponseDto
    {
        public IReadOnlyList<RecipeResponseDto> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
