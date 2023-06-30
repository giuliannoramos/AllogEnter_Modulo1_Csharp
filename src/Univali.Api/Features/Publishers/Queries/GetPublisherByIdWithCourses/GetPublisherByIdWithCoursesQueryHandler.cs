using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetPublisherByIdWithCourses;

public class GetPublisherByIdWithCoursesQueryHandler : IRequestHandler<GetPublisherByIdWithCoursesQuery, GetPublisherByIdWithCoursesDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetPublisherByIdWithCoursesQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<GetPublisherByIdWithCoursesDto> Handle(GetPublisherByIdWithCoursesQuery request, CancellationToken cancellationToken)
    {
        var publisherFromDatabase = await _publisherRepository.GetPublisherByIdWithCoursesAsync(request.PublisherId);
        return _mapper.Map<GetPublisherByIdWithCoursesDto>(publisherFromDatabase);
    }
}