using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Features.RecipeIngredients
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly RecipeLabDbContext _dbContext;

        public RecipeIngredientService(RecipeLabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<RecipeIngredientResponseDto>?> GetForRecipeAsync(string userId, Guid recipeId, CancellationToken cancellationToken)
        {
            var recipeExists = await _dbContext.Recipes
                .AnyAsync(recipe => recipe.Id == recipeId && recipe.UserId == userId, cancellationToken);

            if (!recipeExists)
            {
                return null;
            }

            var recipeIngredients = await _dbContext.RecipeIngredients
                .Where(recipeIngredient =>
                    recipeIngredient.RecipeId == recipeId &&
                    recipeIngredient.Recipe.UserId == userId &&
                    recipeIngredient.Ingredient.UserId == userId)
                .AsNoTracking()
                .OrderBy(recipeIngredient => recipeIngredient.Ingredient.Name)
                .Select(recipeIngredient => new RecipeIngredientResponseDto
                {
                    RecipeId = recipeIngredient.RecipeId,
                    IngredientId = recipeIngredient.IngredientId,
                    IngredientName = recipeIngredient.Ingredient.Name,
                    Quantity = recipeIngredient.Quantity
                })
                .ToListAsync(cancellationToken);

            return recipeIngredients;
        }

        public async Task<RecipeIngredientResponseDto?> GetByIdForRecipeAsync(string userId, Guid recipeId, Guid ingredientId, CancellationToken cancellationToken)
        {
            return await _dbContext.RecipeIngredients
                .AsNoTracking()
                .Where(recipeIngredient =>
                    recipeIngredient.RecipeId == recipeId &&
                    recipeIngredient.IngredientId == ingredientId &&
                    recipeIngredient.Recipe.UserId == userId &&
                    recipeIngredient.Ingredient.UserId == userId)
                .Select(recipeIngredient => new RecipeIngredientResponseDto
                {
                    RecipeId = recipeIngredient.RecipeId,
                    IngredientId = recipeIngredient.IngredientId,
                    IngredientName = recipeIngredient.Ingredient.Name,
                    Quantity = recipeIngredient.Quantity
                })
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<RecipeIngredientAddResult> AddToRecipeAsync(string userId, Guid recipeId, AddRecipeIngredientRequestDto request, CancellationToken cancellationToken)
        {
            var recipeExists = await _dbContext.Recipes
                .AnyAsync(recipe => recipe.Id == recipeId && recipe.UserId == userId, cancellationToken);

            var ingredient = await _dbContext.Ingredients
                .AsNoTracking()
                .SingleOrDefaultAsync(ingredient => ingredient.Id == request.IngredientId && ingredient.UserId == userId, cancellationToken);

            if (!recipeExists || ingredient is null)
            {
                return new RecipeIngredientAddResult
                {
                    Status = RecipeIngredientAddStatus.NotFound
                };
            }

            var associationExists = await _dbContext.RecipeIngredients
                .AnyAsync(recipeIngredient =>
                    recipeIngredient.RecipeId == recipeId &&
                    recipeIngredient.IngredientId == request.IngredientId,
                    cancellationToken);

            if (associationExists)
            {
                return new RecipeIngredientAddResult
                {
                    Status = RecipeIngredientAddStatus.Conflict
                };
            }

            var newRecipeIngredient = new RecipeIngredient
            {
                RecipeId = recipeId,
                IngredientId = request.IngredientId,
                Quantity = request.Quantity
            };

            _dbContext.RecipeIngredients.Add(newRecipeIngredient);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RecipeIngredientAddResult
            {
                Status = RecipeIngredientAddStatus.Created,
                RecipeIngredient = new RecipeIngredientResponseDto
                {
                    RecipeId = newRecipeIngredient.RecipeId,
                    IngredientId = newRecipeIngredient.IngredientId,
                    IngredientName = ingredient.Name,
                    Quantity = newRecipeIngredient.Quantity
                }
            };
        }

        public async Task<RecipeIngredientResponseDto?> UpdateForRecipeAsync(string userId, Guid recipeId, Guid ingredientId, UpdateRecipeIngredientRequestDto request, CancellationToken cancellationToken)
        {
            var recipeIngredient = await _dbContext.RecipeIngredients
                .Include(recipeIngredient => recipeIngredient.Ingredient)
                .SingleOrDefaultAsync(recipeIngredient =>
                    recipeIngredient.RecipeId == recipeId &&
                    recipeIngredient.IngredientId == ingredientId &&
                    recipeIngredient.Recipe.UserId == userId &&
                    recipeIngredient.Ingredient.UserId == userId,
                    cancellationToken);

            if (recipeIngredient is null)
            {
                return null;
            }

            recipeIngredient.Quantity = request.Quantity;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RecipeIngredientResponseDto
            {
                RecipeId = recipeIngredient.RecipeId,
                IngredientId = recipeIngredient.IngredientId,
                IngredientName = recipeIngredient.Ingredient.Name,
                Quantity = recipeIngredient.Quantity
            };
        }

        public async Task<bool> DeleteForRecipeAsync(string userId, Guid recipeId, Guid ingredientId, CancellationToken cancellationToken)
        {
            var recipeIngredient = await _dbContext.RecipeIngredients
                .SingleOrDefaultAsync(recipeIngredient =>
                    recipeIngredient.RecipeId == recipeId &&
                    recipeIngredient.IngredientId == ingredientId &&
                    recipeIngredient.Recipe.UserId == userId &&
                    recipeIngredient.Ingredient.UserId == userId,
                    cancellationToken);

            if (recipeIngredient is null)
            {
                return false;
            }

            _dbContext.RecipeIngredients.Remove(recipeIngredient);

            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
