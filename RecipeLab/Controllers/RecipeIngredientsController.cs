using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLab.Features.RecipeIngredients;
using System.Security.Claims;

namespace RecipeLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeIngredientsController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;

        public RecipeIngredientsController(IRecipeIngredientService recipeIngredientService)
        {
            _recipeIngredientService = recipeIngredientService;
        }

        [HttpGet("recipe/{recipeId:guid}")]
        public async Task<ActionResult<IReadOnlyList<RecipeIngredientResponseDto>>> GetForRecipeAsync(Guid recipeId, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipeIngredients = await _recipeIngredientService.GetForRecipeAsync(userId, recipeId, cancellation);

            if (recipeIngredients is null)
            {
                return NotFound();
            }

            return Ok(recipeIngredients);
        }

        [HttpPost("recipe/{recipeId:guid}")]
        public async Task<ActionResult<RecipeIngredientResponseDto>> AddToRecipeAsync(Guid recipeId, AddRecipeIngredientRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _recipeIngredientService.AddToRecipeAsync(userId, recipeId, request, cancellation);

            if (result.Status == RecipeIngredientAddStatus.NotFound)
            {
                return NotFound();
            }

            if (result.Status == RecipeIngredientAddStatus.Conflict)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(GetForRecipeAsync), new { recipeId }, result.RecipeIngredient);
        }
    }
}
