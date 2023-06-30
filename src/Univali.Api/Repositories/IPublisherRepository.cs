using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public interface IPublisherRepository
{
    //--------------------Author--------------------//
    void AddAuthor(Author author);
    void UpdateAuthor(Author author);
    void DeleteAuthor(Author author);
    Task<Author?> GetAuthorByIdAsync(int authorId);

    //--------------------Course--------------------//
    void AddCourse(Course course);
    void UpdateCourse(Course course);
    void DeleteCourse(Course course);
    Task<Course?> GetCourseByIdAsync(int courseId);
    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course?> GetCourseByIdWithAuthorsAsync(int courseId);

    //--------------------Publisher--------------------//
    void AddPublisher(Publisher publisher);
    Task<Publisher?> GetPublisherByIdAsync(int publisherId);

    //--------------------Global--------------------//
    Task<bool> SaveChangesAsync();
}