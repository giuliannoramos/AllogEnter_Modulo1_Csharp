using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;

namespace Univali.Api.Controller;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var result = 
        (
            new List<Customer>
            {
                new Customer{
                    Id = 1,
                    Name = "Linus", 
                    Cpf = "123456789"
                },
                new Customer{
                    Id = 2,
                    Name = "Bill",
                    Cpf = "987654321"
                }           
                
            }
        );
        
        return Ok(result);
    }
}