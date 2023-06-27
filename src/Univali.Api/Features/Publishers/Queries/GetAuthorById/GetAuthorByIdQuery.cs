using MediatR;

namespace Univali.Api.Features.Publisher.Queries.GetAuthorById;

public class GetAuthorByIdQuery : IRequest<GetAuthorByIdDto>
{
    public int Id {get; set;}
}