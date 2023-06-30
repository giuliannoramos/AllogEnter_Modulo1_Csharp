namespace Univali.Api.Models
{
   public class AuthorWithCoursesDto
   {
      public int AuthorId { get; set; }
      public string FirstName { get; set; } = string.Empty;
      public string LastName { get; set; } = string.Empty;
      public List<CourseDto> Courses { get; set; } = new();
   }
}