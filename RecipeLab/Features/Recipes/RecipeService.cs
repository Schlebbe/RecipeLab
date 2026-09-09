using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Features.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeLabDbContext _dbContext;

        public RecipeService(RecipeLabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<RecipeResponseDto>> GetForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var userRecipes = await _dbContext.Recipes
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAtUtc)
                .Select(r => new RecipeResponseDto
                {
                    CreatedAtUtc = r.CreatedAtUtc,
                    Description = r.Description,
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync(cancellationToken);

            return userRecipes;
        }

        public async Task<RecipeResponseDto> CreateForUserAsync(string userId, CreateRecipeRequestDto request, CancellationToken cancellationToken)
        {
            var recipeName = request.Name.Trim();

            var newRecipe = new Recipe
            {
                CreatedAtUtc = DateTimeOffset.UtcNow,
                Description = request.Description,
                Name = recipeName,
                UserId = userId
            };

            _dbContext.Recipes.Add(newRecipe);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var recipeResponse = new RecipeResponseDto
            {
                CreatedAtUtc = newRecipe.CreatedAtUtc,
                Description = newRecipe.Description,
                Name = newRecipe.Name,
                Id = newRecipe.Id
            };

            return recipeResponse;
        }

        public async Task<RecipeResponseDto?> GetByIdForUserAsync(string userId, Guid recipeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Where(recipe => recipe.Id == recipeId && recipe.UserId == userId)
                .Select(recipe => new RecipeResponseDto
                {
                    Id = recipe.Id,
                    CreatedAtUtc = recipe.CreatedAtUtc,
                    Description = recipe.Description,
                    Name = recipe.Name
                })
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
