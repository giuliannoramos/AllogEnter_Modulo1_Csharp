using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetCourseByIdQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GetCourseByIdDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var courseFromDatabase = await _publisherRepository.GetCourseByIdAsync(request.CourseId);
        return _mapper.Map<GetCourseByIdDto>(courseFromDatabase);
    }
}