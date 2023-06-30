using MediatR;
using Univali.Api.Models;

namespace Univali.Api.Features.Publishers.Queries.GetCourseByIdWithAuthors;

public class GetCourseByIdWithAuthorsQuery : IRequest<GetCourseByIdWithAuthorsDto>
{
    public int CourseId { get; set; }
}