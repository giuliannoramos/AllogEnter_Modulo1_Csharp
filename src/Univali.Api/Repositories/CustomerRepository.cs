using Univali.Api.DbContexts;
using Univali.Api.Entities;

namespace Univali.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext customerContext)
    {
        _context = customerContext;
    }

    public IEnumerable<Customer>GetCustomers()
    {
        return _context.Customers.OrderBy(c => c.Name).ToList();
    }

    public Customer? GetCustomerById(int customerId)
    {
        return  _context.Customers.FirstOrDefault(c => c.Id == customerId);
    }
}