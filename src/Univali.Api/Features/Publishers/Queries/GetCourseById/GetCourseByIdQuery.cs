using MediatR;
using Univali.Api.Models;

namespace Univali.Api.Features.Publishers.Queries.GetCourseById;

public class GetCourseByIdQuery : IRequest<GetCourseByIdDto>
{
    public int CourseId { get; set; }
}