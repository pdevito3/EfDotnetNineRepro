namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Ingredients.Features;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Resources;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using Asp.Versioning;
using MediatR;

[ApiController]
[Route("api/v{v:apiVersion}/ingredients")]
[ApiVersion("1.0")]
public sealed class IngredientsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a list of all Ingredients.
    /// </summary>
    [HttpGet(Name = "GetIngredients")]
    public async Task<IActionResult> GetIngredients([FromQuery] IngredientParametersDto ingredientParametersDto)
    {
        var query = new GetIngredientList.Query(ingredientParametersDto);
        var queryResponse = await mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single Ingredient by ID.
    /// </summary>
    [HttpGet("{ingredientId:guid}", Name = "GetIngredient")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(Guid ingredientId)
    {
        var query = new GetIngredient.Query(ingredientId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Ingredient record.
    /// </summary>
    [HttpPost(Name = "AddIngredient")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IngredientForCreationDto ingredientForCreation)
    {
        var command = new AddIngredient.Command(ingredientForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetIngredient",
            new { ingredientId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Ingredient.
    /// </summary>
    [HttpPut("{ingredientId:guid}", Name = "UpdateIngredient")]
    public async Task<IActionResult> UpdateIngredient(Guid ingredientId, IngredientForUpdateDto ingredient)
    {
        var command = new UpdateIngredient.Command(ingredientId, ingredient);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Ingredient record.
    /// </summary>
    [HttpDelete("{ingredientId:guid}", Name = "DeleteIngredient")]
    public async Task<ActionResult> DeleteIngredient(Guid ingredientId)
    {
        var command = new DeleteIngredient.Command(ingredientId);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Creates one or more Ingredient records.
    /// </summary>
    [HttpPost("batch", Name = "AddIngredientList")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IEnumerable<IngredientForCreationDto> ingredientForCreation,
        [FromQuery(Name = "recipeId"), BindRequired] Guid recipeId)
    {
        var command = new AddIngredientList.Command(ingredientForCreation, recipeId);
        var commandResponse = await mediator.Send(command);
        return Created("GetIngredient", commandResponse);
    }

    // endpoint marker - do not delete this comment
}
