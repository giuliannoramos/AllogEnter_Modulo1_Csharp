using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetCourseByIdWithAuthors;

public class GetCourseByIdWithAuthorsQueryHandler : IRequestHandler<GetCourseByIdWithAuthorsQuery, GetCourseByIdWithAuthorsDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetCourseByIdWithAuthorsQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GetCourseByIdWithAuthorsDto> Handle(GetCourseByIdWithAuthorsQuery request, CancellationToken cancellationToken)
    {
        var courseFromDatabase = await _publisherRepository.GetCourseByIdWithAuthorsAsync(request.CourseId);
        return _mapper.Map<GetCourseByIdWithAuthorsDto>(courseFromDatabase);
    }
}