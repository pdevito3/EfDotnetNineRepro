namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Authors.Features;
using RecipeManagement.Domain.Authors.Dtos;
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
[Route("api/v{v:apiVersion}/authors")]
[ApiVersion("1.0")]
public sealed class AuthorsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a list of all Authors.
    /// </summary>
    [HttpGet(Name = "GetAuthors")]
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorParametersDto authorParametersDto)
    {
        var query = new GetAuthorList.Query(authorParametersDto);
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
    /// Gets a list of all Authors.
    /// </summary>
    [HttpGet("all", Name = "GetAllAuthors")]
    public async Task<IActionResult> GetAllAuthors()
    {
        var query = new GetAllAuthors.Query();
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single Author by ID.
    /// </summary>
    [HttpGet("{authorId:guid}", Name = "GetAuthor")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
    {
        var query = new GetAuthor.Query(authorId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Author record.
    /// </summary>
    [HttpPost(Name = "AddAuthor")]
    public async Task<ActionResult<AuthorDto>> AddAuthor([FromBody]AuthorForCreationDto authorForCreation)
    {
        var command = new AddAuthor.Command(authorForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetAuthor",
            new { authorId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Author.
    /// </summary>
    [HttpPut("{authorId:guid}", Name = "UpdateAuthor")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId, AuthorForUpdateDto author)
    {
        var command = new UpdateAuthor.Command(authorId, author);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Author record.
    /// </summary>
    [HttpDelete("{authorId:guid}", Name = "DeleteAuthor")]
    public async Task<ActionResult> DeleteAuthor(Guid authorId)
    {
        var command = new DeleteAuthor.Command(authorId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
