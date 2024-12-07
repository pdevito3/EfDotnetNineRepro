namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Resources;
using RecipeManagement.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/recipes")]
public sealed class RecipesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a single Recipe by ID.
    /// </summary>
    [HttpGet("{recipeId:guid}", Name = "GetRecipe")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(Guid recipeId)
    {
        var query = new GetRecipe.Query(recipeId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Recipe record.
    /// </summary>
    [HttpPost(Name = "AddRecipe")]
    public async Task<ActionResult<RecipeDto>> AddRecipe([FromBody]RecipeForCreationDto recipeForCreation)
    {
        var command = new AddRecipe.Command(recipeForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetRecipe",
            new { recipeId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Recipe.
    /// </summary>
    [HttpPut("{recipeId:guid}", Name = "UpdateRecipe")]
    public async Task<IActionResult> UpdateRecipe(Guid recipeId, RecipeForUpdateDto recipe)
    {
        var command = new UpdateRecipe.Command(recipeId, recipe);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Recipe record.
    /// </summary>
    [HttpDelete("{recipeId:guid}", Name = "DeleteRecipe")]
    public async Task<ActionResult> DeleteRecipe(Guid recipeId)
    {
        var command = new DeleteRecipe.Command(recipeId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
