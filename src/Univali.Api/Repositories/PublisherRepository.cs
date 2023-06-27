using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly PublisherContext _context;

    public PublisherRepository(PublisherContext publisherContext)
    {
        _context = publisherContext;
    }

    public void AddAuthor(Author author)
    {
        _context.Authors.Add(author);
    }

    public async Task<Author?> GetAuthorByIdAsync(int authorId)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
    }

    public void DeleteAuthor(Author author)
    {
        _context.Authors.Remove(author);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0);
    }    
}