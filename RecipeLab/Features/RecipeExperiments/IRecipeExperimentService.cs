namespace RecipeLab.Features.RecipeExperiments
{
    public interface IRecipeExperimentService
    {
        Task<IReadOnlyList<RecipeExperimentResponseDto>?> GetForRecipeAsync(string userId, Guid recipeId, CancellationToken cancellationToken);
        Task<RecipeExperimentResponseDto?> CreateForRecipeAsync(string userId, Guid recipeId, CreateRecipeExperimentRequestDto request, CancellationToken cancellationToken);
        Task<RecipeExperimentResponseDto?> GetByIdForUserAsync(string userId, Guid experimentId, CancellationToken cancellationToken);
        Task<RecipeExperimentResponseDto?> UpdateForUserAsync(string userId, Guid experimentId, UpdateRecipeExperimentRequestDto request, CancellationToken cancellationToken);
        Task<bool> DeleteForUserAsync(string userId, Guid experimentId, CancellationToken cancellationToken);
    }
}
