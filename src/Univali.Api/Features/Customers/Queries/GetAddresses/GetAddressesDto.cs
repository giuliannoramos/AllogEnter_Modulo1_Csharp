namespace Univali.Api.Features.Customers.Queries.GetAddresses;
public class GetAddressesDto
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}