using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Features.Publishers.Commands.CreateCourse;
using Univali.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Univali.Api.Features.Publishers.Queries.GetCourseById;

namespace Univali.Api.Controllers;

[Route("api/courses")]
[Authorize] //Quando isso é habilitado os métodos precisam de autenticação
public class CoursesController : MainController
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<CourseWithAutorsDto>> AddCourse(CreateCourseCommand createCourseCommand)
    {
        var courseForReturn = await _mediator.Send(createCourseCommand);

        return CreatedAtRoute
        (
            "GetCourseById",
            new { courseId = courseForReturn.CourseId },
            courseForReturn
        );
    }

    [HttpGet("{courseId}", Name = "GetCourseById")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int courseId)
    {
        var getCourseByIdQuery = new GetCourseByIdQuery { CourseId = courseId };
        var courseToReturn = await _mediator.Send(getCourseByIdQuery);
        if (courseToReturn == null) return NotFound();
        return Ok(courseToReturn);
    }
}