using System.Collections.Generic;
using Univali.Api.Entities;

namespace Univali.Api.Features.Publihser.Queries.GetAuthorByIdWithCourses;

public class GetAuthorByIdWithCoursesDto
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;    
    public List<Course> Courses { get; set; } = new ();
}
