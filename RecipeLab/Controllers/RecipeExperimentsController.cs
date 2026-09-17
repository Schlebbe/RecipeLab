using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLab.Features.RecipeExperiments;
using System.Security.Claims;

namespace RecipeLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeExperimentsController : ControllerBase
    {
        private readonly IRecipeExperimentService _recipeExperimentService;

        public RecipeExperimentsController(IRecipeExperimentService recipeExperimentService)
        {
            _recipeExperimentService = recipeExperimentService;
        }

        [HttpGet("recipe/{recipeId:guid}")]
        public async Task<ActionResult<IReadOnlyList<RecipeExperimentResponseDto>>> GetForRecipeAsync(Guid recipeId, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipeExperiments = await _recipeExperimentService.GetForRecipeAsync(userId, recipeId, cancellation);

            if (recipeExperiments is null)
            {
                return NotFound();
            }

            return Ok(recipeExperiments);
        }

        [HttpPost("recipe/{recipeId:guid}")]
        public async Task<ActionResult<RecipeExperimentResponseDto>> CreateForRecipeAsync(Guid recipeId, CreateRecipeExperimentRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipeExperiment = await _recipeExperimentService.CreateForRecipeAsync(userId, recipeId, request, cancellation);

            if (recipeExperiment is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = recipeExperiment.Id }, recipeExperiment);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecipeExperimentResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipeExperiment = await _recipeExperimentService.GetByIdForUserAsync(userId, id, cancellationToken);

            if (recipeExperiment is null)
            {
                return NotFound();
            }

            return Ok(recipeExperiment);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RecipeExperimentResponseDto>> UpdateByIdAsync(Guid id, UpdateRecipeExperimentRequestDto request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updatedRecipeExperiment = await _recipeExperimentService.UpdateForUserAsync(userId, id, request, cancellationToken);

            if (updatedRecipeExperiment is null)
            {
                return NotFound();
            }

            return Ok(updatedRecipeExperiment);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _recipeExperimentService.DeleteForUserAsync(userId, id, cancellationToken);

            return result ? NoContent() : NotFound();
        }
    }
}
