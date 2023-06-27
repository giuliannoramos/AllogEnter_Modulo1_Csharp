using AutoMapper;
using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, GetAuthorByIdDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<GetAuthorByIdDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var authorFromDatabase = await _publisherRepository.GetAuthorByIdAsync(request.Id);
        return _mapper.Map<GetAuthorByIdDto>(authorFromDatabase);
    }
}