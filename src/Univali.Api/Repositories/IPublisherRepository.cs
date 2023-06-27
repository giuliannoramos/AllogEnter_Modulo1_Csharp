using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public interface IPublisherRepository
{    
    void AddAuthor(Author author);

    Task<Author?> GetAuthorByIdAsync(int authorId);

    void DeleteAuthor(Author author);

    Task<bool> SaveChangesAsync(); 
}