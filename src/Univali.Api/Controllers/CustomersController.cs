using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;

namespace Univali.Api.Controller;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var result = Data.Instance.Customers;

        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomerById(int id)
    {
        Console.WriteLine($"id: {id}");
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        if(customer == null)
        {
            return NotFound();
        } 

        return Ok(customer);        
    }
    
    [HttpGet("cpf/{cpf}")] 
    public ActionResult<Customer> GetCustomerByCpf(string cpf)
    {
        Console.WriteLine($"cpf: {cpf}");
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);

        if(customer == null)
        {
            return NotFound();
        } 

        return Ok(customer);        
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer([FromBody] Customer customer)
    {
        var newCustomer = new Customer
        {
            Id = Data.Instance.Customers.Max(c => c.Id) + 1,
            Name = customer.Name,
            Cpf = customer.Cpf
        };

        Data.Instance.Customers.Add(newCustomer);
        return CreatedAtRoute
        (
            "GetCustomerById",
            new {id = newCustomer.Id},
            newCustomer
        );
    }
}

