using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Univali.Api.Features.Publishers.Commands.UpdateCourse;

public class UpdateCourseCommand : IRequest
{

    [Required(ErrorMessage = "You should fill out an CourseId")]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "You should fill out a Title")]
    [MaxLength(30, ErrorMessage = "The title shouldn't have more than 30 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a Description")]
    [MaxLength(50, ErrorMessage = "The description shouldn't have more than 20 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a Price")]
    public decimal Price { get; set; }
}