namespace Univali.Api.Features.Authors.Commands.CreateAuthor;

public class CreateAuthorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}