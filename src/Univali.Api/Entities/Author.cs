namespace Univali.Api.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Course> Courses {get; set; } = new();

    public Author(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"Id: {AuthorId}\nFirstName: {FirstName}\nLastName: {LastName}";
    }
}