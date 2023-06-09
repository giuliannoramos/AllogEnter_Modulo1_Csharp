using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Features.Publishers.Commands.CreateAuthor;
using Univali.Api.Features.Publishers.Commands.DeleteAuthor;
using Univali.Api.Features.Publishers.Commands.UpdateAuthor;
using Univali.Api.Features.Publishers.Queries.GetAuthorById;
using Univali.Api.Features.Publishers.Queries.GetAuthorByIdWithCourses;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/authors")]
[Authorize] //Quando isso é habilitado os métodos precisam de autenticação
public class AuthorsController : MainController
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorCommand createAuthorCommand)
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
    public async Task<ActionResult<AuthorDto>> GetAuthorById(int authorId)
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

    [HttpPut("{authorId}")]
    public async Task<ActionResult> UpdateAuthor(int authorId, [FromBody] UpdateAuthorCommand updateAuthorCommand)
    {
        if (authorId != updateAuthorCommand.AuthorId) BadRequest();

        await _mediator.Send(updateAuthorCommand);

        return NoContent();
    }

    [HttpGet("{authorId}/courses")]
    public async Task<ActionResult<AuthorWithCoursesDto>> GetAuthorWithCourses(int authorId)
    {
        var getAuthorByIdWithCoursesQuery = new GetAuthorByIdWithCoursesQuery { AuthorId = authorId };
        var authorToReturn = await _mediator.Send(getAuthorByIdWithCoursesQuery);
        if (authorToReturn == null) return NotFound();
        return Ok(authorToReturn);
    }

}