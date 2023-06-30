// using AutoMapper;
// using MediatR;
// using Univali.Api.Features.Publihser.Queries.GetAuthorByIdWithCourses;
// using Univali.Api.Features.Publishers.Queries.GetCourseById;
// using Univali.Api.Repositories;

// namespace Univali.Api.Features.Publishers.Queries.GetAuthorByIdWithCourses
// {
//     public class GetAuthorByIdWithCoursesQueryHandler : IRequestHandler<GetAuthorByIdWithCoursesQuery, GetAuthorByIdWithCoursesDto>
//     {
//         private readonly IPublisherRepository _publisherRepository;
//         private readonly IMapper _mapper;

//         public GetAuthorByIdWithCoursesQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
//         {
//             _publisherRepository = publisherRepository;
//             _mapper = mapper;
//         }

//         public async Task<GetAuthorByIdWithCoursesDto> Handle(GetAuthorByIdWithCoursesQuery request, CancellationToken cancellationToken)
//         {
//             var authorFromDatabase = await _publisherRepository.GetAuthorByIdWithCoursesAsync(request.AuthorId);
//             if (authorFromDatabase == null)
//             {
//                 // Retornar null ou lançar uma exceção adequada se o autor não for encontrado
//                 return null;
//             }

//             var authorDto = _mapper.Map<GetAuthorByIdWithCoursesDto>(authorFromDatabase);

//             // Carregar os cursos do autor
//             var courses = await _publisherRepository.GetCoursesByAuthorIdAsync(request.AuthorId);

//             authorDto.Courses = _mapper.Map<List<GetCourseByIdDto>>(courses);

//             return authorDto;
//         }
//     }
// }