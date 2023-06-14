namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;

public interface IGetCustomerWithAddressesQueryHandler
{
    Task<GetCustomerWithAddressesDto?> HandleCustomerWithAddresses(GetCustomerWithAddressesQuery request);    
}