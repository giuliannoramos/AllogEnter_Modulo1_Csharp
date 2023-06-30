using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.UpdateAuthor;


public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        // Obtém a entidade do cliente pelo ID
        // ?? é uma expressão para simplificar uma estrutura de if caso o autor seja nulo, retornar not found
        var authorEntity = await _publisherRepository.GetAuthorByIdAsync(request.AuthorId) ?? throw new InvalidOperationException("Author not found");

        // Mapeia as propriedades do comando de atualização para a entidade do autor
        _mapper.Map(request, authorEntity);

        _publisherRepository.UpdateAuthor(authorEntity);

        // Salva as alterações no repositório
        await _publisherRepository.SaveChangesAsync();
    }
}