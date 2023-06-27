using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Authors.Commands.CreateAuthor;

// IRequest<> transforma a classe em uma Mensagem
// CreateCustomerDto é o tipo que este comando espera receber de volta
public class CreateAuthorCommand : IRequest<CreateAuthorDto>
{
    [Required(ErrorMessage = "You should fill out a first name")]
    [MaxLength(20, ErrorMessage = "The first name shouldn't have more than 20 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a last name")]
    [MaxLength(20, ErrorMessage = "The last name shouldn't have more than 20 characters")]
    public string LastName { get; set; } = string.Empty;
}