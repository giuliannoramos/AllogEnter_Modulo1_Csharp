using AutoMapper;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.CreateCourse;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCourseDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {

        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));

    }

    public async Task<CreateCourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        // Mapear o objeto CreateCourseCommand para um objeto Course
        var courseEntity = _mapper.Map<Course>(request);

        // Limpar a lista de autores do curso (caso exista)
        courseEntity.Authors.Clear();

        // Percorrer a lista de autores fornecida no comando
        foreach (var author in request.Authors)
        {
            // Obter o autor do banco de dados pelo seu ID
            var authorDB = await _publisherRepository.GetAuthorByIdAsync(author.AuthorId);

            // Verificar se o autor existe no banco de dados
            if (authorDB != null)
            {
                // Verificar se o autor já está associado ao curso
                if (!courseEntity.Authors.Any(a => a.AuthorId == authorDB.AuthorId))
                {
                    // Adicionar o autor à lista de autores do curso
                    courseEntity.Authors.Add(authorDB);
                }
            }
        }

        // Adicionar o curso ao repositório
        _publisherRepository.AddCourse(courseEntity);

        // Salvar as alterações no banco de dados
        await _publisherRepository.SaveChangesAsync();

        // Mapear o objeto Course de volta para CreateCourseDto
        var courseToReturn = _mapper.Map<CreateCourseDto>(courseEntity);
        return courseToReturn;
    }

}