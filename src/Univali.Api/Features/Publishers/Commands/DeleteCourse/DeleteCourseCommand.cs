using MediatR;

namespace Univali.Api.Features.Publishers.Commands.DeleteCourse;

public class DeleteCourseCommand : IRequest<bool>
{
    public int CourseId { get; set; }
}