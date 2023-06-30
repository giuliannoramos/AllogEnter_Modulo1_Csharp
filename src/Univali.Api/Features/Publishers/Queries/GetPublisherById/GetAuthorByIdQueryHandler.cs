using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetPublisherById;

public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, GetPublisherByIdDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetPublisherByIdQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<GetPublisherByIdDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var PublisherFromDatabase = await _publisherRepository.GetPublisherByIdAsync(request.PublisherId);
        return _mapper.Map<GetPublisherByIdDto>(PublisherFromDatabase);
    }
}