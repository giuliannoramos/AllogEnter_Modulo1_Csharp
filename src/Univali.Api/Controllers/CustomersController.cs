using Microsoft.AspNetCore.JsonPatch;
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
    public ActionResult<IEnumerable<CustomerCreateDto>> GetCustomers()
    {
        // var customerFromDataBase = Data.Instance.Customers;

        // // Lista de clientes a serem retornados
        // List<CustomerDto> customerRetorno = new List<CustomerDto>();

        // // Percorre todos os clientes e adiciona na lista de retorno
        // foreach (var customer in customerFromDataBase)
        // {
        //     customerRetorno.Add(new CustomerDto
        //     {
        //         Name = customer.Name,
        //         Cpf = customer.Cpf
        //     });
        // }

        // return Ok(customerRetorno);

        var customersToReturn = Data.Instance.Customers
            .Select(customer => new CustomerReturnDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf
            });
        return Ok(customersToReturn);
    }


    /// <summary>
    /// Obtém um cliente da lista "Customers" no Singleton com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do cliente a ser recuperado.</param>
    /// <returns>Um código de status HTTP 200 (OK) juntamente com o DTO do cliente encontrado, 
    /// ou um código de status HTTP 404 (Not Found) se o cliente não for encontrado.</returns>
    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerCreateDto> GetCustomerById(int id)
    {
        Console.WriteLine($"id: {id}");

        // Procura um cliente com o ID especificado
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Cria um DTO de cliente para retornar
        var customerToReturn = new CustomerReturnDto
        {
            Id = customerFromDataBase.Id,
            Name = customerFromDataBase.Name,
            Cpf = customerFromDataBase.Cpf
        };

        return Ok(customerToReturn);
    }


    /// <summary>
    /// Obtém um cliente da lista "Customers" do Singleton com base no CPF fornecido.
    /// </summary>
    /// <param name="cpf">O CPF do cliente a ser obtido.</param>
    /// <returns>Um código de status HTTP 200 (OK) juntamente com o DTO do cliente encontrado, ou um código 
    /// de status HTTP 404 (Not Found) caso o cliente não seja encontrado.</returns>
    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerCreateDto> GetCustomerByCpf(string cpf)
    {
        Console.WriteLine($"cpf: {cpf}");

        // Procura um cliente com o CPF especificado
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);

        // Verifica se o cliente foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Cria um DTO de cliente para retornar
        var customerToReturn = new CustomerReturnDto
        {
            Id = customerFromDataBase.Id,
            Name = customerFromDataBase.Name,
            Cpf = customerFromDataBase.Cpf
        };

        return Ok(customerToReturn);
    }


    /// <summary>
    /// Cria um novo cliente na lista "Customers" do Singleton com base nos dados fornecidos.
    /// </summary>
    /// <param name="customerDto">O objeto contendo as informações do novo cliente.</param>
    /// <returns>Um código de status HTTP 201 (Created) juntamente com o DTO do cliente criado.</returns>
    [HttpPost]
    public IActionResult CreateCustomer(CustomerCreateDto customerReturnDto)
    {
        // Cria um objeto de cliente a partir do DTO recebido
        var customer = new Customer
        {
            Id = Data.Instance.GenerateCustomerId(),
            Name = customerReturnDto.Name,
            Cpf = customerReturnDto.Cpf
        };

        // Adiciona o cliente à instância de dados
        Data.Instance.AddCustomer(customer);

        // Cria um DTO de cliente como resposta
        var customerToReturn = new CustomerReturnDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf
        };

        return CreatedAtAction("GetCustomerById", new { id = customerToReturn.Id }, customerToReturn);
    }


    /// <summary>
    /// Atualiza as informações de um cliente na lista "Customers" do Singleton.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
    /// <param name="customerDto">O objeto contendo as novas informações do cliente.</param>
    /// <returns>Um código de status HTTP indicando o sucesso ou fracasso da operação.</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, CustomerCreateDto customerCreateDto)
    {
        Console.WriteLine($"id: {id}");

        // Procura o cliente com o ID fornecido na lista "Customers"
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customerFromDataBase == null)
        {
            // Retorna um código de status HTTP 404 (Not Found) indicando que o cliente não foi encontrado
            return NotFound();
        }

        // Atualiza os dados do cliente com base no objeto CustomerDto recebido
        customerFromDataBase.Name = customerCreateDto.Name;
        customerFromDataBase.Cpf = customerCreateDto.Cpf;

        // Retorna um código de status HTTP 204 (No Content) para indicar sucesso na atualização sem retornar dados adicionais
        return NoContent();
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
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customerFromDataBase == null)
        {
            // Retorna um código de status HTTP 404 (Not Found) indicando que o cliente não foi encontrado
            return NotFound();
        }

        // Remove o cliente da lista "Customers"
        Data.Instance.Customers.Remove(customerFromDataBase);

        // Retorna um código de status HTTP 204 (No Content) indicando que a remoção foi realizada com sucesso
        return NoContent();
    }


    /// <summary>
    /// Método para atualizar parcialmente um cliente.
    /// </summary>
    /// <param name="patchDocument">Documento JSON com as alterações a serem aplicadas no cliente.</param>
    /// <param name="id">O ID do cliente a ser atualizado.</param>
    /// <returns>Um objeto ActionResult representando o resultado da atualização parcial.</returns>
    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
    [FromBody] JsonPatchDocument<CustomerCreateDto> patchDocument,
    [FromRoute] int id)
    {
        // Obter o cliente do banco de dados com base no ID fornecido.
        var customerFromDataBase = Data.Instance.Customers
            .FirstOrDefault(customer => customer.Id == id);

        // Verificar se o cliente foi encontrado.
        if (customerFromDataBase == null)
            return NotFound();

        // Criar um objeto CustomerCreateDto com as propriedades do cliente a serem atualizadas.
        var customerToPatch = new CustomerCreateDto
        {
            Name = customerFromDataBase.Name,
            Cpf = customerFromDataBase.Cpf
        };

        // Aplicar as alterações do patchDocument no customerToPatch.
        patchDocument.ApplyTo(customerToPatch);

        // Atualizar as propriedades do cliente no banco de dados com base nas alterações aplicadas.
        customerFromDataBase.Name = customerToPatch.Name;
        customerFromDataBase.Cpf = customerToPatch.Cpf;

        // Retornar um resultado sem conteúdo para indicar que a atualização parcial foi bem-sucedida.
        return NoContent();
    }

}