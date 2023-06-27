using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Features.Publishers.Commands.CreateAuthor;
using Univali.Api.Features.Publishers.Commands.DeleteAuthor;
using Univali.Api.Features.Publishers.Queries.GetAuthorById;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/authors")]
[Authorize]
public class AuthorsController : MainController
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(
        CreateAuthorCommand createAuthorCommand
        )
    {
        var authorToReturn = await _mediator.Send(createAuthorCommand);

        return CreatedAtRoute
        (
            "GetAuthorById",
            new { authorId = authorToReturn.AuthorId },
            authorToReturn
        );
    }

    [HttpGet("{authorId}", Name = "GetAuthorById")]
    public async Task<ActionResult<AuthorDto>> GetAuthorById(
    int authorId)
    {
        var getAuthorByIdQuery = new GetAuthorByIdQuery { AuthorId = authorId };

        var authorToReturn = await _mediator.Send(getAuthorByIdQuery);

        if (authorToReturn == null) return NotFound();

        return Ok(authorToReturn);
    }

    [HttpDelete("{authorId}")]
    public async Task<IActionResult> DeleteAuthor(int authorId)
    {
        var deleteAuthorCommand = new DeleteAuthorCommand { AuthorId = authorId };

        var result = await _mediator.Send(deleteAuthorCommand);

        if (!result) return NotFound(); // Autor não encontrado

        return NoContent(); // Autor excluído com sucesso
    }

}