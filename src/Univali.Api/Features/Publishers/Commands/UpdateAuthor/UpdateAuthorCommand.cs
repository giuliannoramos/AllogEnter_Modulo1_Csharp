using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Publishers.Commands.UpdateAuthor;

public class UpdateAuthorCommand : IRequest
{

    [Required(ErrorMessage = "You should fill out an AuthorId")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "You should fill out a First Name")]
    [MaxLength(20, ErrorMessage = "The first name shouldn't have more than 20 characters")]
    public string FirstName { get; set; } = string.Empty;


    [Required(ErrorMessage = "You should fill out a Last Name")]
    [MaxLength(20, ErrorMessage = "The last name shouldn't have more than 20 characters")]
    public string LastName { get; set; } = string.Empty;
}