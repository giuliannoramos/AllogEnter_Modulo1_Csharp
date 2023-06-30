using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.DeletePublisher;

public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand, bool>
{
    private readonly IPublisherRepository _publisherRepository;

    public DeletePublisherCommandHandler(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<bool> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisherToDelete = await _publisherRepository.GetPublisherByIdAsync(request.PublisherId);

        if (publisherToDelete == null)
            return false; // Autor não encontrado

        _publisherRepository.DeletePublisher(publisherToDelete);
        await _publisherRepository.SaveChangesAsync();

        return true; // Autor excluído com sucesso
    }
}