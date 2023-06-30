using Univali.Api.Models;

namespace Univali.Api.Features.Publishers.Queries.GetPublisherByIdWithCourses;

public class GetPublisherByIdWithCoursesDto
{
    public int PublisherId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;    
    public List<CourseDto> Courses { get; set; } = new ();
}
