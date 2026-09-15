namespace RecipeLab.Features.Ingredients
{
    public interface IIngredientService
    {
        Task<IReadOnlyList<IngredientResponseDto>> GetForUserAsync(string userId, CancellationToken cancellationToken);
        Task<IngredientResponseDto> CreateForUserAsync(string userId, CreateIngredientRequestDto request, CancellationToken cancellationToken);
        Task<IngredientResponseDto?> GetByIdForUserAsync(string userId, Guid ingredientId, CancellationToken cancellationToken);
        Task<IngredientResponseDto?> UpdateForUserAsync(string userId, Guid ingredientId, UpdateIngredientRequestDto request, CancellationToken cancellationToken);
        Task<bool> DeleteForUserAsync(string userId, Guid ingredientId, CancellationToken cancellationToken);
    }
}
