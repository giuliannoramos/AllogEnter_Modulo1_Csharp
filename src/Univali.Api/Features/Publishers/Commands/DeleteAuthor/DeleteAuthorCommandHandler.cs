using MediatR;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Publishers.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IPublisherRepository _publisherRepository;

        public DeleteAuthorCommandHandler(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToDelete = await _publisherRepository.GetAuthorByIdAsync(request.AuthorId);

            if (authorToDelete == null)
                return false; // Autor não encontrado

            _publisherRepository.DeleteAuthor(authorToDelete);
            await _publisherRepository.SaveChangesAsync();

            return true; // Autor excluído com sucesso
        }
    }
}