using AutoMapper;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.CreateAuthor;

// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class CreateAuthorCommandHandler: IRequestHandler<CreateAuthorCommand, CreateAuthorDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<CreateAuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = _mapper.Map<Author>(request);
        _publisherRepository.AddAuthor(authorEntity);
        await _publisherRepository.SaveChangesAsync();
        var authorToReturn = _mapper.Map<CreateAuthorDto>(authorEntity);
        return authorToReturn;
    }
}