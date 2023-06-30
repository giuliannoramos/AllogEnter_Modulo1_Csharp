using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.UpdatePublisher;


public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public UpdatePublisherCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        // Obtém a entidade pelo ID
        // ?? é uma expressão para simplificar uma estrutura de if caso seja nulo, retornar not found
        var publisherEntity = await _publisherRepository.GetPublisherByIdAsync(request.PublisherId) ?? throw new InvalidOperationException("Publisher not found");

        // Mapeia as propriedades do comando de atualização para a entidade do autor
        _mapper.Map(request, publisherEntity);

        _publisherRepository.UpdatePublisher(publisherEntity);

        // Salva as alterações no repositório
        await _publisherRepository.SaveChangesAsync();
    }
}