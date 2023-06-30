using MediatR;

namespace Univali.Api.Features.Publishers.Queries.GetPublisherByIdWithCourses;

public class GetPublisherByIdWithCoursesQuery : IRequest<GetPublisherByIdWithCoursesDto>
{
    public int PublisherId { get; set; }
}