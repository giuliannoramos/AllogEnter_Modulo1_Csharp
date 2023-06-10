using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<Customer?> GetCustomerByCpfAsync(string Cpf)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Cpf == Cpf);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync()
    {
        return await _context.Customers.Include(c => c.Addresses).ToListAsync();
    }

    public async Task<Customer?> GetCustomerWithAddressesByIdAsync(int customerId)
    {
        return await _context.Customers.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<Address?> GetAddressAsync(int addressId)
    {
        return await _context.Addresses.FirstOrDefaultAsync(address => address.Id == addressId);
    }
}