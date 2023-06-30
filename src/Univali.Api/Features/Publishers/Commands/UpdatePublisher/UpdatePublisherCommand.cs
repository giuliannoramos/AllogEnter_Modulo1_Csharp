using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Publishers.Commands.UpdatePublisher;

public class UpdatePublisherCommand : IRequest
{

    [Required(ErrorMessage = "You should fill out an AuthorId")]
    public int PublisherId { get; set; }

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