namespace Univali.Api.Features.Publishers.Commands.CreatePublisher;

public class CreatePublisherDto
{
    public int PublisherId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}