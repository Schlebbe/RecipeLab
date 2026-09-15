using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Features.Ingredients
{
    public class IngredientService : IIngredientService
    {
        private readonly RecipeLabDbContext _dbContext;

        public IngredientService(RecipeLabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<IngredientResponseDto>> GetForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var userIngredients = await _dbContext.Ingredients
                .Where(i => i.UserId == userId)
                .AsNoTracking()
                .OrderBy(i => i.Name)
                .Select(i => new IngredientResponseDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync(cancellationToken);

            return userIngredients;
        }

        public async Task<IngredientResponseDto> CreateForUserAsync(string userId, CreateIngredientRequestDto request, CancellationToken cancellationToken)
        {
            var ingredientName = request.Name.Trim();

            var newIngredient = new Ingredient
            {
                Name = ingredientName,
                UserId = userId
            };

            _dbContext.Ingredients.Add(newIngredient);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var ingredientResponse = new IngredientResponseDto
            {
                Id = newIngredient.Id,
                Name = newIngredient.Name
            };

            return ingredientResponse;
        }

        public async Task<IngredientResponseDto?> GetByIdForUserAsync(string userId, Guid ingredientId, CancellationToken cancellationToken)
        {
            return await _dbContext.Ingredients
                .AsNoTracking()
                .Where(ingredient => ingredient.Id == ingredientId && ingredient.UserId == userId)
                .Select(ingredient => new IngredientResponseDto
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name
                })
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IngredientResponseDto?> UpdateForUserAsync(string userId, Guid ingredientId, UpdateIngredientRequestDto request, CancellationToken cancellationToken)
        {
            var ingredient = await _dbContext.Ingredients.SingleOrDefaultAsync(ingredient => ingredient.UserId == userId && ingredient.Id == ingredientId, cancellationToken);

            if (ingredient is null)
            {
                return null;
            }

            ingredient.Name = request.Name.Trim();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new IngredientResponseDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }

        public async Task<bool> DeleteForUserAsync(string userId, Guid ingredientId, CancellationToken cancellationToken)
        {
            var ingredient = await _dbContext.Ingredients.SingleOrDefaultAsync(ingredient => ingredient.Id == ingredientId && ingredient.UserId == userId, cancellationToken);

            if (ingredient is null)
            {
                return false;
            }

            _dbContext.Ingredients.Remove(ingredient);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
