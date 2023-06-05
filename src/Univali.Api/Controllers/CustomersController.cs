using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly Data _data;
    private readonly IMapper _mapper;

    public CustomersController(Data data, IMapper mapper)
    {
        _data = data ?? throw new ArgumentException(nameof(data));
        _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var customersFromDatabase = _data.Customers;
        var customersToReturn = _mapper.Map<IEnumerable<CustomerDto>>(customersFromDatabase);

        return Ok(customersToReturn);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        var customerFromDatabase = _data
            .Customers.FirstOrDefault(c => c.Id == id);

        if (customerFromDatabase == null) return NotFound();

        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(c => c.Cpf == cpf);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        return Ok(customerToReturn);
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            // Cria a fábrica de um objeto de detalhes de problema de validação
            var problemDetailsFactory = HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();

            // Cria um objeto de detalhes de problema de validação
            var validationProblemDetails = problemDetailsFactory
                .CreateValidationProblemDetails(HttpContext, ModelState);

            // Atribui o status code 422 no corpo do response
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

            return UnprocessableEntity(validationProblemDetails);
        }

        var customerEntity = new Customer()
        {
            Id = _data.Customers.Max(c => c.Id) + 1,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };

        _data.Customers.Add(customerEntity);

        var customerToReturn = new CustomerDto
        {
            Id = customerEntity.Id,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };

        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id, CustomerForUpdateDto customerForUpdateDto)
    {
        if (id != customerForUpdateDto.Id) return BadRequest();

        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        _mapper.Map(customerForUpdateDto, customerFromDatabase);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(int id)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        _data.Customers.Remove(customerFromDatabase);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer([FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id)
    {
        var customerFromDatabase = _data.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto
        {
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();

    }

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses()
    {
        var customersFromDatabase = _data.Customers;

        var customersToReturn = customersFromDatabase
            .Select(customer => new CustomerWithAddressesDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf,
                Addresses = customer.Addresses
                    .Select(address => new AddressDto
                    {
                        Id = address.Id,
                        City = address.City,
                        Street = address.Street
                    }).ToList()
            });

        return Ok(customersToReturn);
    }

    [HttpPost("with-addresses")]
    public ActionResult<CustomerWithAddressesDto> CreateCustomerWithAddresses([FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Cria uma nova entidade de cliente
        var customerEntity = new Customer
        {
            Id = _data.Customers.Max(c => c.Id) + 1,
            Name = customerWithAddressesCreateDto.Name,
            Cpf = customerWithAddressesCreateDto.Cpf
        };

        // Cria entidades de endereço para o cliente
        var addressEntities = new List<Address>();

        // Obtém o maior ID de todos os endereços existentes
        var maxAddressId = _data.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

        int newId = 1;

        foreach (var address in customerWithAddressesCreateDto.Addresses)
        {
            var addressEntity = new Address
            {
                Id = maxAddressId + newId, // Define o ID adicionando o máximo existente e o contador
                Street = address.Street,
                City = address.City,
            };
            addressEntities.Add(addressEntity);
            newId++;
        }

        // Adiciona os endereços à entidade de cliente
        foreach (var address in addressEntities)
        {
            customerEntity.Addresses.Add(address);
        }

        // Adiciona o cliente à fonte de dados (_data)
        _data.Customers.Add(customerEntity);

        // Mapeia a entidade de cliente para o DTO a ser retornado
        var customerWithAddressesToReturn = new CustomerWithAddressesDto
        {
            Id = customerEntity.Id,
            Name = customerEntity.Name,
            Cpf = customerEntity.Cpf,
            Addresses = addressEntities.Select(address => new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
            }).ToList()
        };

        return CreatedAtAction("GetCustomersWithAddresses", new { customerId = customerWithAddressesToReturn.Id }, customerWithAddressesToReturn);
    }

    [HttpPut("with-addresses/{customerId}")]
    public IActionResult UpdateCustomerWithAddresses(int customerId, [FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Verifica se o cliente existe
        var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Atualiza os dados do cliente
        customerFromDataBase.Name = customerWithAddressesCreateDto.Name;
        customerFromDataBase.Cpf = customerWithAddressesCreateDto.Cpf;

        // Remove todos os endereços existentes do cliente
        customerFromDataBase.Addresses.Clear();

        // Cria novas entidades de endereço para o cliente
        var addressEntities = new List<Address>();

        // Obtém o maior ID de todos os endereços existentes
        var maxAddressId = _data.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

        int newId = 1;

        foreach (var address in customerWithAddressesCreateDto.Addresses)
        {
            var addressEntity = new Address
            {
                Id = maxAddressId + newId, // Define o ID adicionando o máximo existente e o contador
                Street = address.Street,
                City = address.City,
            };
            addressEntities.Add(addressEntity);
            newId++;
        }

        // Adiciona os novos endereços ao cliente
        foreach (var address in addressEntities)
        {
            customerFromDataBase.Addresses.Add(address);
        }

        return NoContent();
    }

}