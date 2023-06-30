using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetAuthorByIdWithCourses;

public class GetAuthorByIdWithCoursesQueryHandler : IRequestHandler<GetAuthorByIdWithCoursesQuery, GetAuthorByIdWithCoursesDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdWithCoursesQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<GetAuthorByIdWithCoursesDto> Handle(GetAuthorByIdWithCoursesQuery request, CancellationToken cancellationToken)
    {
        var authorFromDatabase = await _publisherRepository.GetAuthorByIdWithCoursesAsync(request.AuthorId);
        return _mapper.Map<GetAuthorByIdWithCoursesDto>(authorFromDatabase);
    }
}