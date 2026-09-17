using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Features.RecipeExperiments
{
    public class RecipeExperimentService : IRecipeExperimentService
    {
        private readonly RecipeLabDbContext _dbContext;

        public RecipeExperimentService(RecipeLabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<RecipeExperimentResponseDto>?> GetForRecipeAsync(string userId, Guid recipeId, CancellationToken cancellationToken)
        {
            var recipeExists = await _dbContext.Recipes
                .AnyAsync(recipe => recipe.Id == recipeId && recipe.UserId == userId, cancellationToken);

            if (!recipeExists)
            {
                return null;
            }

            var recipeExperiments = await _dbContext.RecipeExperiments
                .Where(experiment => experiment.RecipeId == recipeId && experiment.UserId == userId)
                .AsNoTracking()
                .OrderByDescending(experiment => experiment.CreatedAtUtc)
                .Select(experiment => new RecipeExperimentResponseDto
                {
                    Id = experiment.Id,
                    RecipeId = experiment.RecipeId,
                    PreparationMethod = experiment.PreparationMethod,
                    VariationNotes = experiment.VariationNotes,
                    ResultNotes = experiment.ResultNotes,
                    Rating = experiment.Rating,
                    CreatedAtUtc = experiment.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return recipeExperiments;
        }

        public async Task<RecipeExperimentResponseDto?> CreateForRecipeAsync(string userId, Guid recipeId, CreateRecipeExperimentRequestDto request, CancellationToken cancellationToken)
        {
            var recipeExists = await _dbContext.Recipes
                .AnyAsync(recipe => recipe.Id == recipeId && recipe.UserId == userId, cancellationToken);

            if (!recipeExists)
            {
                return null;
            }

            var newRecipeExperiment = new RecipeExperiment
            {
                RecipeId = recipeId,
                UserId = userId,
                PreparationMethod = request.PreparationMethod.Trim(),
                VariationNotes = request.VariationNotes,
                ResultNotes = request.ResultNotes,
                Rating = request.Rating,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            _dbContext.RecipeExperiments.Add(newRecipeExperiment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RecipeExperimentResponseDto
            {
                Id = newRecipeExperiment.Id,
                RecipeId = newRecipeExperiment.RecipeId,
                PreparationMethod = newRecipeExperiment.PreparationMethod,
                VariationNotes = newRecipeExperiment.VariationNotes,
                ResultNotes = newRecipeExperiment.ResultNotes,
                Rating = newRecipeExperiment.Rating,
                CreatedAtUtc = newRecipeExperiment.CreatedAtUtc
            };
        }

        public async Task<RecipeExperimentResponseDto?> GetByIdForUserAsync(string userId, Guid experimentId, CancellationToken cancellationToken)
        {
            return await _dbContext.RecipeExperiments
                .AsNoTracking()
                .Where(experiment => experiment.Id == experimentId && experiment.UserId == userId)
                .Select(experiment => new RecipeExperimentResponseDto
                {
                    Id = experiment.Id,
                    RecipeId = experiment.RecipeId,
                    PreparationMethod = experiment.PreparationMethod,
                    VariationNotes = experiment.VariationNotes,
                    ResultNotes = experiment.ResultNotes,
                    Rating = experiment.Rating,
                    CreatedAtUtc = experiment.CreatedAtUtc
                })
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<RecipeExperimentResponseDto?> UpdateForUserAsync(string userId, Guid experimentId, UpdateRecipeExperimentRequestDto request, CancellationToken cancellationToken)
        {
            var experiment = await _dbContext.RecipeExperiments
                .SingleOrDefaultAsync(experiment => experiment.Id == experimentId && experiment.UserId == userId, cancellationToken);

            if (experiment is null)
            {
                return null;
            }

            experiment.PreparationMethod = request.PreparationMethod.Trim();
            experiment.VariationNotes = request.VariationNotes;
            experiment.ResultNotes = request.ResultNotes;
            experiment.Rating = request.Rating;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RecipeExperimentResponseDto
            {
                Id = experiment.Id,
                RecipeId = experiment.RecipeId,
                PreparationMethod = experiment.PreparationMethod,
                VariationNotes = experiment.VariationNotes,
                ResultNotes = experiment.ResultNotes,
                Rating = experiment.Rating,
                CreatedAtUtc = experiment.CreatedAtUtc
            };
        }

        public async Task<bool> DeleteForUserAsync(string userId, Guid experimentId, CancellationToken cancellationToken)
        {
            var experiment = await _dbContext.RecipeExperiments
                .SingleOrDefaultAsync(experiment => experiment.Id == experimentId && experiment.UserId == userId, cancellationToken);

            if (experiment is null)
            {
                return false;
            }

            _dbContext.RecipeExperiments.Remove(experiment);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
