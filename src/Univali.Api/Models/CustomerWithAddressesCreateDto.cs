namespace Univali.Api.Models;

public class CustomerWithAddressesCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}