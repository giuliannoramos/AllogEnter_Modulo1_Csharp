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

    [HttpGet("{id}")]
    public ActionResult<Customer> GetCustomer([FromRoute] int id)
    {
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        if(customer == null)
        {
            return NotFound();
        }
        
        return Ok(customer);        
    }
}

