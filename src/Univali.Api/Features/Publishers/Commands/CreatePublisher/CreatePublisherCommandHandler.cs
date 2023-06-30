using AutoMapper;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.CreatePublisher;

// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class CreatePublisherCommandHandler: IRequestHandler<CreatePublisherCommand, CreatePublisherDto>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public CreatePublisherCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<CreatePublisherDto> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisherEntity = _mapper.Map<Publisher>(request);
        _publisherRepository.AddPublisher(publisherEntity);
        await _publisherRepository.SaveChangesAsync();
        var publisherToReturn = _mapper.Map<CreatePublisherDto>(publisherEntity);
        return publisherToReturn;
    }
}