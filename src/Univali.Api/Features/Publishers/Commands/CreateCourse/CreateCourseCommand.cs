using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Publishers.Commands.CreateCourse;

public class CreateCourseCommand : IRequest<CreateCourseDto>
{
    [Required(ErrorMessage = "You should give a Title")]
    [MaxLength(60, ErrorMessage = "The title shouldn't have more than 60 characters")]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should give a Price")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "You should give a author")]
    public List<AuthorDto> Authors { get; set; } = new();
}

