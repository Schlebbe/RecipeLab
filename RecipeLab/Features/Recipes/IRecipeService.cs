namespace RecipeLab.Features.Recipes
{
    public interface IRecipeService
    {
        Task<IReadOnlyList<RecipeResponseDto>> GetForUserAsync(string userId, CancellationToken cancellationToken);
        Task<RecipeResponseDto> CreateForUserAsync(string userId, CreateRecipeRequestDto request, CancellationToken cancellationToken);
        Task<RecipeResponseDto?> GetByIdForUserAsync(string userId, Guid recipeId, CancellationToken cancellationToken);
        Task<RecipeResponseDto?> UpdateForUserAsync(string userId, Guid recipeId, UpdateRecipeRequestDto request, CancellationToken cancellationToken);
    }
}
