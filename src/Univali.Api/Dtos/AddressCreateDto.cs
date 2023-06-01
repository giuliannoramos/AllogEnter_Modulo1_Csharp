namespace Univali.Api.Dtos
{
    public class AddressCreateDto
    {
        public int CustomerId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}