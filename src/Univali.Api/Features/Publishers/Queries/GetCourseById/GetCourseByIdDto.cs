using Univali.Api.Models;

namespace Univali.Api.Features.Publishers.Queries.GetCourseById;


public class GetCourseByIdDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    //public List<AuthorDto> Authors { get; set; } = null!;
}