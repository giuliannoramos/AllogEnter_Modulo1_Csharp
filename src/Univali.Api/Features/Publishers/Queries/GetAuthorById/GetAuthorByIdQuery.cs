using MediatR;

namespace Univali.Api.Features.Publishers.Queries.GetAuthorById;

public class GetAuthorByIdQuery : IRequest<GetAuthorByIdDto>
{
    public int Id {get; set;}
}