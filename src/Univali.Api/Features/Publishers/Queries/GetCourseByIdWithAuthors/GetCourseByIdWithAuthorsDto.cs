using Univali.Api.Models;


namespace Univali.Api.Features.Publishers.Queries.GetCourseByIdWithAuthors;

public class GetCourseByIdWithAuthorsDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<AuthorDto> Authors { get; set; } = null!;
}