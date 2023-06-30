using MediatR;

namespace Univali.Api.Features.Publishers.Commands.DeleteAuthor;

public class DeleteAuthorCommand : IRequest<bool>
{
    public int AuthorId { get; set; }
}