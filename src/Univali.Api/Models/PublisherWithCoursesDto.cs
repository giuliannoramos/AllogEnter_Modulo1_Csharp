namespace Univali.Api.Models
{
   public class PublisherWithCoursesDto
   {
       public int PublisherId { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string Cpf { get; set; } = string.Empty;
       public List<CourseDto> Courses { get; set; } = new();
   }
}