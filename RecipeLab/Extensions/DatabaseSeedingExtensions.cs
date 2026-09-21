using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;
using RecipeLab.Infrastructure.Identity;
using RecipeLab.Infrastructure.Persistence;

namespace RecipeLab.Extensions
{
    public static class DatabaseSeedingExtensions
    {
        private const string TestUserEmail = "test@example.com";
        private const string TestUserPasswordConfigurationKey = "SeedData:TestUserPassword";

        public static async Task SeedDevelopmentDataAsync(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                return;
            }

            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecipeLabDbContext>();

            var testUser = await GetOrCreateTestUserAsync(userManager, app.Configuration);
            var recipes = await SeedRecipesAsync(dbContext, testUser.Id);
            var ingredients = await SeedIngredientsAsync(dbContext, testUser.Id);

            await SeedRecipeIngredientsAsync(dbContext, recipes, ingredients);
            await SeedExperimentsAsync(dbContext, testUser.Id, recipes);

            app.Logger.LogInformation("Development seed data is available for {Email}.", TestUserEmail);
        }

        private static async Task<ApplicationUser> GetOrCreateTestUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var existingUser = await userManager.FindByEmailAsync(TestUserEmail);

            if (existingUser is not null)
            {
                return existingUser;
            }

            var password = configuration[TestUserPasswordConfigurationKey];

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException($"{TestUserPasswordConfigurationKey} is required to create the development test user.");
            }

            var testUser = new ApplicationUser
            {
                UserName = TestUserEmail,
                Email = TestUserEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(testUser, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"The development test user could not be created: {errors}");
            }

            return testUser;
        }

        private static async Task<Dictionary<string, Recipe>> SeedRecipesAsync(RecipeLabDbContext dbContext, string userId)
        {
            var recipeDefinitions = new[]
            {
                new
                {
                    Name = "Seeded Tomato Pasta",
                    Description = "A simple pasta recipe for development data.",
                    CreatedAtUtc = DateTimeOffset.UtcNow.AddDays(-14),
                },
                new
                {
                    Name = "Seeded Berry Pancakes",
                    Description = "Fluffy pancakes with berries for development data.",
                    CreatedAtUtc = DateTimeOffset.UtcNow.AddDays(-7),
                },
            };

            var existingRecipes = await dbContext.Recipes
                .Where(recipe => recipe.UserId == userId)
                .ToListAsync();

            var recipesByName = new Dictionary<string, Recipe>();
            var recipesToAdd = new List<Recipe>();

            foreach (var definition in recipeDefinitions)
            {
                var recipe = existingRecipes.FirstOrDefault(existingRecipe => existingRecipe.Name == definition.Name);

                if (recipe is null)
                {
                    recipe = new Recipe
                    {
                        UserId = userId,
                        Name = definition.Name,
                        Description = definition.Description,
                        CreatedAtUtc = definition.CreatedAtUtc,
                    };

                    recipesToAdd.Add(recipe);
                }

                recipesByName[definition.Name] = recipe;
            }

            if (recipesToAdd.Count > 0)
            {
                dbContext.Recipes.AddRange(recipesToAdd);
                await dbContext.SaveChangesAsync();
            }

            return recipesByName;
        }

        private static async Task<Dictionary<string, Ingredient>> SeedIngredientsAsync(RecipeLabDbContext dbContext, string userId)
        {
            var ingredientDefinitions = new[]
            {
                "Pasta",
                "Tomatoes",
                "Berries",
                "Flour",
            };

            var existingIngredients = await dbContext.Ingredients
                .Where(ingredient => ingredient.UserId == userId)
                .ToListAsync();

            var ingredientsByName = new Dictionary<string, Ingredient>();
            var ingredientsToAdd = new List<Ingredient>();

            foreach (var name in ingredientDefinitions)
            {
                var ingredient = existingIngredients.FirstOrDefault(existingIngredient => existingIngredient.Name == name);

                if (ingredient is null)
                {
                    ingredient = new Ingredient
                    {
                        UserId = userId,
                        Name = name,
                    };

                    ingredientsToAdd.Add(ingredient);
                }

                ingredientsByName[name] = ingredient;
            }

            if (ingredientsToAdd.Count > 0)
            {
                dbContext.Ingredients.AddRange(ingredientsToAdd);
                await dbContext.SaveChangesAsync();
            }

            return ingredientsByName;
        }

        private static async Task SeedRecipeIngredientsAsync(RecipeLabDbContext dbContext, Dictionary<string, Recipe> recipes, Dictionary<string, Ingredient> ingredients)
        {
            var recipeIngredientDefinitions = new[]
            {
                new { RecipeName = "Seeded Tomato Pasta", IngredientName = "Pasta", Quantity = "200 g" },
                new { RecipeName = "Seeded Tomato Pasta", IngredientName = "Tomatoes", Quantity = "3" },
                new { RecipeName = "Seeded Berry Pancakes", IngredientName = "Berries", Quantity = "150 g" },
                new { RecipeName = "Seeded Berry Pancakes", IngredientName = "Flour", Quantity = "250 g" },
            };

            var recipeIds = recipes.Values.Select(recipe => recipe.Id).ToList();
            var ingredientIds = ingredients.Values.Select(ingredient => ingredient.Id).ToList();
            var existingRecipeIngredients = await dbContext.RecipeIngredients
                .Where(recipeIngredient => recipeIds.Contains(recipeIngredient.RecipeId) && ingredientIds.Contains(recipeIngredient.IngredientId))
                .ToListAsync();

            var recipeIngredientsToAdd = new List<RecipeIngredient>();

            foreach (var definition in recipeIngredientDefinitions)
            {
                var recipe = recipes[definition.RecipeName];
                var ingredient = ingredients[definition.IngredientName];
                var alreadyExists = existingRecipeIngredients.Any(recipeIngredient =>
                    recipeIngredient.RecipeId == recipe.Id && recipeIngredient.IngredientId == ingredient.Id);

                if (alreadyExists)
                {
                    continue;
                }

                var recipeIngredient = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Id,
                    Quantity = definition.Quantity,
                };

                recipeIngredientsToAdd.Add(recipeIngredient);
                existingRecipeIngredients.Add(recipeIngredient);
            }

            if (recipeIngredientsToAdd.Count > 0)
            {
                dbContext.RecipeIngredients.AddRange(recipeIngredientsToAdd);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedExperimentsAsync(RecipeLabDbContext dbContext, string userId, Dictionary<string, Recipe> recipes)
        {
            var experimentDefinitions = new[]
            {
                new
                {
                    RecipeName = "Seeded Tomato Pasta",
                    PreparationMethod = "Stovetop",
                    VariationNotes = "Used fresh tomatoes instead of canned tomatoes.",
                    ResultNotes = "Fresh and light, but needed more seasoning.",
                    Rating = 4,
                    CreatedAtUtc = DateTimeOffset.UtcNow.AddDays(-5),
                },
                new
                {
                    RecipeName = "Seeded Berry Pancakes",
                    PreparationMethod = "Cast iron pan",
                    VariationNotes = "Added berries directly to the batter.",
                    ResultNotes = "Soft pancakes with evenly distributed berries.",
                    Rating = 5,
                    CreatedAtUtc = DateTimeOffset.UtcNow.AddDays(-2),
                },
            };

            var recipeIds = recipes.Values.Select(recipe => recipe.Id).ToList();
            var existingExperiments = await dbContext.RecipeExperiments
                .Where(experiment => experiment.UserId == userId && recipeIds.Contains(experiment.RecipeId))
                .ToListAsync();

            var experimentsToAdd = new List<RecipeExperiment>();

            foreach (var definition in experimentDefinitions)
            {
                var recipe = recipes[definition.RecipeName];
                var alreadyExists = existingExperiments.Any(experiment =>
                    experiment.RecipeId == recipe.Id && experiment.PreparationMethod == definition.PreparationMethod);

                if (alreadyExists)
                {
                    continue;
                }

                var experiment = new RecipeExperiment
                {
                    RecipeId = recipe.Id,
                    UserId = userId,
                    PreparationMethod = definition.PreparationMethod,
                    VariationNotes = definition.VariationNotes,
                    ResultNotes = definition.ResultNotes,
                    Rating = definition.Rating,
                    CreatedAtUtc = definition.CreatedAtUtc,
                };

                experimentsToAdd.Add(experiment);
                existingExperiments.Add(experiment);
            }

            if (experimentsToAdd.Count > 0)
            {
                dbContext.RecipeExperiments.AddRange(experimentsToAdd);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
