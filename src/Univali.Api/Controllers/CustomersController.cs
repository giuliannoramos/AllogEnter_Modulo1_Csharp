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
}