using MediatR;

namespace Univali.Api.Features.Publishers.Queries.GetAuthorByIdWithCourses;

public class GetAuthorByIdWithCoursesQuery : IRequest<GetAuthorByIdWithCoursesDto>
{
    public int AuthorId { get; set; }
}