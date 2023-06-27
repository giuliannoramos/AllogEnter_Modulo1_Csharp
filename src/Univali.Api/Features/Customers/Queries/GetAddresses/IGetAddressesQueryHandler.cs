namespace Univali.Api.Features.Customers.Queries.GetAddresses;
public interface IGetAddressesQueryHandler
{
    Task<List<GetAddressesDto>> HandleGetAddresses(GetAddressesQuery request);
}
