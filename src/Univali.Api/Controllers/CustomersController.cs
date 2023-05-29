using Microsoft.AspNetCore.Mvc;
using Univali.Api.Dtos;
using Univali.Api.Entities;

namespace Univali.Api.Controller;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    /// <summary>
    /// Obtém todos os clientes da lista "Customers" no Singleton.
    /// </summary>
    /// <returns>Um código de status HTTP 200 (OK) juntamente com a lista de DTOs dos clientes encontrados.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var result = Data.Instance.Customers;

        // Lista de clientes a serem retornados
        List<CustomerDto> customerRetorno = new List<CustomerDto>();

        // Percorre todos os clientes e adiciona na lista de retorno
        foreach (var customer in result)
        {
            customerRetorno.Add(new CustomerDto
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Cpf = customer.Cpf
            });
        }

        return Ok(customerRetorno);
    }

    /// <summary>
    /// Obtém um cliente da lista "Customers" no Singleton com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do cliente a ser recuperado.</param>
    /// <returns>Um código de status HTTP 200 (OK) juntamente com o DTO do cliente encontrado, 
    /// ou um código de status HTTP 404 (Not Found) se o cliente não for encontrado.</returns>
    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        Console.WriteLine($"id: {id}");

        // Procura um cliente com o ID especificado
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Cria um DTO de cliente para retornar
        var customerDto = new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Cpf = customer.Cpf
        };

        return Ok(customerDto);
    }

    /// <summary>
    /// Obtém um cliente da lista "Customers" do Singleton com base no CPF fornecido.
    /// </summary>
    /// <param name="cpf">O CPF do cliente a ser obtido.</param>
    /// <returns>Um código de status HTTP 200 (OK) juntamente com o DTO do cliente encontrado, ou um código 
    /// de status HTTP 404 (Not Found) caso o cliente não seja encontrado.</returns>
    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        Console.WriteLine($"cpf: {cpf}");

        // Procura um cliente com o CPF especificado
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);

        // Verifica se o cliente foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Cria um DTO de cliente para retornar
        var customerDto = new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Cpf = customer.Cpf
        };

        return Ok(customerDto);
    }

    /// <summary>
    /// Cria um novo cliente na lista "Customers" do Singleton com base nos dados fornecidos.
    /// </summary>
    /// <param name="customerDto">O objeto contendo as informações do novo cliente.</param>
    /// <returns>Um código de status HTTP 201 (Created) juntamente com o DTO do cliente criado.</returns>
    [HttpPost]
    public IActionResult CreateCustomer(CustomerDto customerDto)
    {
        // Cria um objeto de cliente a partir do DTO recebido
        var customer = new Customer
        {
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            Cpf = customerDto.Cpf
        };

        // Adiciona o cliente à instância de dados
        Data.Instance.AddCustomer(customerDto);

        // Cria um DTO de cliente como resposta
        var customerDtoResponse = new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Cpf = customer.Cpf
        };

        return CreatedAtAction("GetCustomerById", new { id = customer.Id }, customerDtoResponse);
    }

    /// <summary>
    /// Atualiza as informações de um cliente na lista "Customers" do Singleton.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
    /// <param name="customerDto">O objeto contendo as novas informações do cliente.</param>
    /// <returns>Um código de status HTTP indicando o sucesso ou fracasso da operação e o DTO atualizado do cliente.</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, CustomerDto customerDto)
    {
        Console.WriteLine($"id: {id}");

        // Procura o cliente com o ID fornecido na lista "Customers"
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customer == null)
        {
            // Retorna um código de status HTTP 404 (Not Found) indicando que o cliente não foi encontrado
            return NotFound();
        }

        // Atualiza os dados do cliente com base no objeto CustomerDto recebido
        customer.FirstName = customerDto.FirstName;
        customer.LastName = customerDto.LastName;
        customer.Cpf = customerDto.Cpf;

        // Cria um DTO atualizado do cliente
        var updatedCustomerDto = new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Cpf = customer.Cpf
        };

        // Retorna um código de status HTTP 200 (OK) junto com o DTO atualizado do cliente
        return Ok(updatedCustomerDto);
    }

    /// <summary>
    /// Remove um cliente da lista "Customers" do Singleton com base no seu id.
    /// </summary>
    /// <param name="id">O identificador único do cliente para ser removido.</param>
    /// <returns>Um código de status HTTP indicando o sucesso ou fracasso da operação.</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        Console.WriteLine($"id: {id}");

        // Procura o cliente com o ID fornecido na lista "Customers"
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customer == null)
        {
            // Retorna um código de status HTTP 404 (Not Found) indicando que o cliente não foi encontrado
            return NotFound();
        }

        // Remove o cliente da lista "Customers"
        Data.Instance.Customers.Remove(customer);

        // Retorna um código de status HTTP 204 (No Content) indicando que a remoção foi realizada com sucesso
        return NoContent();
    }
}