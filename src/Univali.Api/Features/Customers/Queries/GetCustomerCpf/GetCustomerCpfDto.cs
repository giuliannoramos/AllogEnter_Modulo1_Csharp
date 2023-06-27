namespace Univali.Api.Features.Customers.Queries.GetCustomerCpf;


public class GetCustomerCpfDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}