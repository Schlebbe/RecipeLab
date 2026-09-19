namespace RecipeLab.Features.RecipeIngredients
{
    public interface IRecipeIngredientService
    {
        Task<IReadOnlyList<RecipeIngredientResponseDto>?> GetForRecipeAsync(string userId, Guid recipeId, CancellationToken cancellationToken);
        Task<RecipeIngredientAddResult> AddToRecipeAsync(string userId, Guid recipeId, AddRecipeIngredientRequestDto request, CancellationToken cancellationToken);
        Task<RecipeIngredientResponseDto?> UpdateForRecipeAsync(string userId, Guid recipeId, Guid ingredientId, UpdateRecipeIngredientRequestDto request, CancellationToken cancellationToken);
    }
}
