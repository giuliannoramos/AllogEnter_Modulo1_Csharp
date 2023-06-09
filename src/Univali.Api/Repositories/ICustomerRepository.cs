using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public interface ICustomerRepository
{
    IEnumerable<Customer>GetCustomers();  

    Customer? GetCustomerById(int customerId);  
}