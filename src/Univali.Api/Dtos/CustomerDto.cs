namespace Univali.Api.Dtos;

public class CustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName
    {
        get
        {
            return FirstName + " " + LastName;
        }
    }
    public string Cpf { get; set; } = string.Empty;
}