using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.DeleteCourse;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly IPublisherRepository _publisherRepository;

    public DeleteCourseCommandHandler(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var courseToDelete = await _publisherRepository.GetCourseByIdAsync(request.CourseId);

        if (courseToDelete == null)
            return false; // Não encontrado

        _publisherRepository.DeleteCourse(courseToDelete);
        await _publisherRepository.SaveChangesAsync();

        return true; // Excluído com sucesso
    }
}