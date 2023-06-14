namespace Univali.Api.Features.Customers.Queries.GetCustomerCpf;

public interface IGetCustomerCpfQueryHandler
{
    Task<GetCustomerCpfDto?> HandleGetCpf(GetCustomerCpfQuery request);
}