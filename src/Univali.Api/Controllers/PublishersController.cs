using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Univali.Api.Features.Publishers.Commands.CreatePublisher;
using Univali.Api.Features.Publishers.Queries.GetPublisherById;

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

    [HttpGet("{PublisherId}", Name = "GetPublisherById")]
    public async Task<ActionResult<PublisherDto>> GetPublisherById(int PublisherId)
    {
        var getPublisherByIdQuery = new GetPublisherByIdQuery { PublisherId = PublisherId };

        var publisherToReturn = await _mediator.Send(getPublisherByIdQuery);

        if (publisherToReturn == null) return NotFound();

        return Ok(publisherToReturn);
    }
}