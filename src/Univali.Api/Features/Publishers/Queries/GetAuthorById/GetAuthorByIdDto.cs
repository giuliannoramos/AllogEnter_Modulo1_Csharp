namespace Univali.Api.Features.Publisher.Queries.GetAuthorById;


public class GetAuthorByIdDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}