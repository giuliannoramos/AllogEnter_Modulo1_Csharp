using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Univali.Api.Features.Publishers.Commands.CreatePublisher;
using Univali.Api.Features.Publishers.Queries.GetPublisherById;
using Univali.Api.Features.Publishers.Commands.DeletePublisher;
using Univali.Api.Features.Publishers.Commands.UpdatePublisher;
using Univali.Api.Features.Publishers.Queries.GetPublisherByIdWithCourses;

namespace Univali.Api.Controllers;

[Route("api/publishers")]
[Authorize] //Quando isso é habilitado os métodos precisam de autenticação
public class PublishersController : MainController
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<PublisherDto>> AddPublisher(CreatePublisherCommand createPublisherCommand)
    {
        var publisherForReturn = await _mediator.Send(createPublisherCommand);

        return CreatedAtRoute
        (
            "GetPublisherById",
            new { publisherId = publisherForReturn.PublisherId },
            publisherForReturn
        );
    }

    [HttpGet("{publisherId}", Name = "GetPublisherById")]
    public async Task<ActionResult<PublisherDto>> GetPublisherById(int PublisherId)
    {
        var getPublisherByIdQuery = new GetPublisherByIdQuery { PublisherId = PublisherId };

        var publisherToReturn = await _mediator.Send(getPublisherByIdQuery);

        if (publisherToReturn == null) return NotFound();

        return Ok(publisherToReturn);
    }

    [HttpPut("{publisherId}")]
    public async Task<ActionResult> UpdatePublisher(int publisherId, [FromBody] UpdatePublisherCommand updatePublisherCommand)
    {
        if (publisherId != updatePublisherCommand.PublisherId) BadRequest();

        await _mediator.Send(updatePublisherCommand);

        return NoContent();
    }

    [HttpDelete("{publisherId}")]
    public async Task<IActionResult> DeletePublisher(int publisherId)
    {
        var deletePublisherCommand = new DeletePublisherCommand { PublisherId = publisherId };

        var result = await _mediator.Send(deletePublisherCommand);

        if (!result) return NotFound(); // Não encontrado

        return NoContent(); // Excluído com sucesso
    }

    [HttpGet("{publisherId}/with-courses")]
    public async Task<ActionResult<PublisherWithCoursesDto>> GetPublisherByIdWithCourses(int publisherId)
    {
        var getPublisherByIdWithCoursesQuery = new GetPublisherByIdWithCoursesQuery { PublisherId = publisherId };
        var courseToReturn = await _mediator.Send(getPublisherByIdWithCoursesQuery);
        if (courseToReturn == null) return NotFound();
        return Ok(courseToReturn);
    }

}