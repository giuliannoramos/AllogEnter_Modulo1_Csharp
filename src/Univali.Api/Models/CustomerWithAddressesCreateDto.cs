namespace Univali.Api.Models;

public class CustomerWithAddressesCreateDto : CustomerForManipulationDto
{    
    public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}