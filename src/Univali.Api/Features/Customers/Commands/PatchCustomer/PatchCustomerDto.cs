using System.ComponentModel.DataAnnotations;

namespace Univali.Api.Features.Customers.Commands.PatchCustomer
{
    public class PatchCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}