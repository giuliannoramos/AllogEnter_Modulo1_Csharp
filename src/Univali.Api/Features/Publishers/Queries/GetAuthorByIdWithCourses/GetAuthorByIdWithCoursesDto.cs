using Univali.Api.Models;

namespace Univali.Api.Features.Publishers.Queries.GetAuthorByIdWithCourses;

public class GetAuthorByIdWithCoursesDto
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;    
    public List<CourseDto> Courses { get; set; } = new ();
}
