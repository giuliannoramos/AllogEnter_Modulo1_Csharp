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

    //--------------------Author--------------------//
    public void AddAuthor(Author author)
    {
        // Os metodos abaixo vão excluir e recriar o banco de dados, aplicando todas as migrações pendentes.
        // _context.Database.EnsureDeleted();
        // _context.Database.Migrate();
        _context.Authors.Add(author);
    }

    public void UpdateAuthor(Author author)
    {
        _context.Authors.Update(author);
    }

    public void DeleteAuthor(Author author)
    {
        _context.Authors.Remove(author);
    }

    public async Task<Author?> GetAuthorByIdAsync(int authorId)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
    }

    //--------------------Course--------------------//
    public void AddCourse(Course course)
    {
        _context.Courses.Add(course);
    }

    public void UpdateCourse(Course course)
    {
        throw new NotImplementedException();
    }

    public void DeleteCourse(Course course)
    {
        throw new NotImplementedException();
    }

    public async Task<Course?> GetCourseByIdAsync(int courseId)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
    }

    public Task<IEnumerable<Course>> GetCoursesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Course?> GetCourseByIdWithAuthorsAsync(int courseId)
    {
        throw new NotImplementedException();
    }

    //--------------------Publisher--------------------//
    public void AddPublisher(Publisher publisher)
    {
        _context.Publishers.Add(publisher);
    }

    public async Task<Publisher?> GetPublisherByIdAsync(int publisherId)
    {
        return await _context.Publishers.FirstOrDefaultAsync(p => p.PublisherId == publisherId);
    }

    //--------------------Global--------------------//
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}