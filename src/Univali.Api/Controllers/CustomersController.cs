using Microsoft.AspNetCore.Mvc;

namespace Univali.Api.Controller;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    public JsonResult GetCustomers()
    {
        return new JsonResult
        (
            new List<object>
            {
                new
                {
                    Id = 1,
                    Name = "Linus",
                    Cpf = "123456789"
                },
                new
                {
                    Id = 2,
                    Name = "Bill",
                    Cpf = "987654321"
                }           
                
            }
        );
    }
}