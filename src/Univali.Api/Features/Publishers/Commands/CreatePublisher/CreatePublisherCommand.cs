using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Publishers.Commands.CreatePublisher;

// IRequest<> transforma a classe em uma Mensagem
// CreateCustomerDto é o tipo que este comando espera receber de volta
public class CreatePublisherCommand : IRequest<CreatePublisherDto>
{
    [Required(ErrorMessage = "You should fill out a first name")]
    [MaxLength(20, ErrorMessage = "The first name shouldn't have more than 20 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a last name")]
    [MaxLength(20, ErrorMessage = "The last name shouldn't have more than 20 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a Cpf")]
    [CpfMustBeValid(ErrorMessage = "The provided {0} should be valid number.")]
    public string Cpf { get; set; } = string.Empty;
}