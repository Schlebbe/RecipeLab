namespace RecipeLab.Features.Recipes
{
    public interface IRecipeService
    {
        Task<IReadOnlyList<RecipeResponseDto>> GetForUserAsync(string userId, CancellationToken cancellation);
    }
}
