using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<RecipeResponseDto>> GetForUserAsync(string userId, CancellationToken cancellation)
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
                .ToListAsync(cancellation);

            return userRecipes;
        }
    }
}
