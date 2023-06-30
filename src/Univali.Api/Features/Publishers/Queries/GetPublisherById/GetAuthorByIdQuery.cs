using MediatR;

namespace Univali.Api.Features.Publishers.Queries.GetPublisherById;

public class GetPublisherByIdQuery : IRequest<GetPublisherByIdDto>
{
    public int PublisherId {get; set;}
}