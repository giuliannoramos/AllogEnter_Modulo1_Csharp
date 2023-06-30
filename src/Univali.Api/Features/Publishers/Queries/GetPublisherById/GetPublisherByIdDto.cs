namespace Univali.Api.Features.Publishers.Queries.GetPublisherById;


public class GetPublisherByIdDto
{
    public int PublisherId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}