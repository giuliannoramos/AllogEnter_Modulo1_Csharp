using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.UpdateCourse;


public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        // Obtém a entidade pelo ID
        // ?? é uma expressão para simplificar uma estrutura de if caso seja nulo, retornar not found
        var courseEntity = await _publisherRepository.GetCourseByIdAsync(request.CourseId) ?? throw new InvalidOperationException("Course not found");

        // Mapeia as propriedades do comando de atualização para a entidade do autor
        _mapper.Map(request, courseEntity);

        _publisherRepository.UpdateCourse(courseEntity);

        // Salva as alterações no repositório
        await _publisherRepository.SaveChangesAsync();
    }
}