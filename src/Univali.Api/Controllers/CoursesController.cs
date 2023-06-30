using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Features.Publishers.Commands.CreateCourse;
using Univali.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Univali.Api.Features.Publishers.Queries.GetCourseById;
using Univali.Api.Features.Publishers.Queries.GetCourseByIdWithAuthors;
using Univali.Api.Features.Publishers.Commands.DeleteCourse;
using Univali.Api.Features.Publishers.Commands.UpdateCourse;

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

    [HttpPut("{courseId}")]
    public async Task<ActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseCommand updateCourseCommand)
    {
        if (courseId != updateCourseCommand.CourseId) BadRequest();

        await _mediator.Send(updateCourseCommand);

        return NoContent();
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> DeleteCourse(int courseId)
    {
        var deleteCourseCommand = new DeleteCourseCommand { CourseId = courseId };

        var result = await _mediator.Send(deleteCourseCommand);

        if (!result) return NotFound(); // Não encontrado

        return NoContent(); // Excluído com sucesso
    }

    [HttpGet("{courseId}/authors")]
    public async Task<ActionResult<CourseWithAutorsDto>> GetCourseWithAuthors(int courseId)
    {
        var getCourseByIdWithAuthorsQuery = new GetCourseByIdWithAuthorsQuery { CourseId = courseId };
        var courseToReturn = await _mediator.Send(getCourseByIdWithAuthorsQuery);
        if (courseToReturn == null) return NotFound();
        return Ok(courseToReturn);
    }
}