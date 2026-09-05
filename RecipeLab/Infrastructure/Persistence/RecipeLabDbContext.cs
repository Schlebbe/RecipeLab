using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeLab.Domain;

namespace RecipeLab.Infrastructure.Persistence
{
    public class RecipeLabDbContext : IdentityDbContext
    {
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public DbSet<RecipeExperiment> RecipeExperiments => Set<RecipeExperiment>();

        public RecipeLabDbContext(DbContextOptions<RecipeLabDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(recipe => recipe.Id);

                entity.Property(recipe => recipe.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(recipe => recipe.Name)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(recipe => recipe.Description)
                    .HasMaxLength(2000);

                entity.HasIndex(recipe => new
                {
                    recipe.UserId,
                    recipe.Name
                });

                entity.HasMany(recipe => recipe.RecipeIngredients)
                    .WithOne(recipeIngredient => recipeIngredient.Recipe)
                    .HasForeignKey(recipeIngredient => recipeIngredient.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(recipe => recipe.Experiments)
                    .WithOne(experiment => experiment.Recipe)
                    .HasForeignKey(experiment => experiment.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(ingredient => ingredient.Id);

                entity.Property(ingredient => ingredient.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(ingredient => ingredient.Name)
                    .IsRequired()
                    .HasMaxLength(120);
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(recipeIngredient => new
                {
                    recipeIngredient.RecipeId,
                    recipeIngredient.IngredientId
                });

                entity.Property(recipeIngredient => recipeIngredient.Quantity)
                    .HasMaxLength(80);

                entity.HasOne(recipeIngredient => recipeIngredient.Ingredient)
                    .WithMany(ingredient => ingredient.RecipeIngredients)
                    .HasForeignKey(recipeIngredient => recipeIngredient.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeExperiment>(entity =>
            {
                entity.HasKey(experiment => experiment.Id);

                entity.Property(experiment => experiment.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(experiment => experiment.PreparationMethod)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(experiment => experiment.VariationNotes)
                    .HasMaxLength(2000);

                entity.Property(experiment => experiment.ResultNotes)
                    .HasMaxLength(2000);

                entity.Property(experiment => experiment.Rating)
                    .IsRequired();

                entity.ToTable(table =>
                    table.HasCheckConstraint(
                        "CK_RecipeExperiment_Rating",
                        "[Rating] BETWEEN 1 AND 5"));
            });
        }
    }
}
