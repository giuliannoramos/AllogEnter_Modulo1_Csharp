using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressController : ControllerBase
{

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        // // Procurar o cliente com o ID fornecido
        // var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // // Verificar se o cliente não foi encontrado
        // if (customerFromDataBase == null)
        // {
        //     return NotFound();
        // }

        // // Procurar o endereço com o ID fornecido dentro do cliente encontrado
        // var addressToReturn = customerFromDataBase.Addresses.FirstOrDefault(a => a.Id == addressId);

        // // Verificar se o endereço não foi encontrado
        // if (addressToReturn == null)
        // {
        //     return NotFound();
        // }

        // // Retornar o endereço encontrado com o status HTTP 200 OK
        // return Ok(addressToReturn);

        {
            var addressToReturn = Data.Instance
                .Customers.FirstOrDefault(customer => customer.Id == customerId)
                ?.Addresses.FirstOrDefault(address => address.Id == addressId);

            return addressToReturn != null ? Ok(addressToReturn) : NotFound();
        }
    }

    [HttpPost]
    public ActionResult<AddressDto> CreateAddress(int customerId, [FromBody] AddressForCreationDto addressForCreationDto)
    {
        // Procura o cliente com o ID fornecido
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Obtém o maior ID de todos os endereços existentes
        var maxAddressId = Data.Instance.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

        int newId = 1; //contador

        // Cria uma nova entidade de endereço
        var addressEntity = new Address()
        {
            Id = maxAddressId + newId, // Maior Id de todos e o contador
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        // Adiciona o novo endereço à lista de endereços do cliente
        customerFromDataBase.Addresses.Add(addressEntity);

        // Mapeia os atributos do novo endereço para o DTO de resposta
        var addressToReturn = new AddressDto
        {
            Id = addressEntity.Id,
            Street = addressEntity.Street,
            City = addressEntity.City
        };

        return CreatedAtAction("GetAddress", new { customerId = customerId, addressId = addressToReturn.Id }, addressToReturn);
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateAddress(int customerId, int addressId, [FromBody] AddressForUpdateDto addressForUpdateDto)
    {
        // Verifica se o ID do endereço fornecido no corpo da requisição corresponde ao ID fornecido como parâmetro
        if (addressId != addressForUpdateDto.Id)
            return BadRequest();

        // Procura o cliente com o ID fornecido
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
            return NotFound();

        // Procura o endereço com o ID fornecido dentro do cliente
        var addressToUpdate = customerFromDataBase.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (addressToUpdate == null)
            return NotFound();

        // Atualiza os atributos do endereço com base nos dados fornecidos
        addressToUpdate.Street = addressForUpdateDto.Street;
        addressToUpdate.City = addressForUpdateDto.City;

        // Retorna o status 204 No Content para indicar que a atualização foi bem-sucedida
        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verificar se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Procurar o endereço com o ID fornecido dentro do cliente encontrado
        var addressToDelete = customerFromDataBase.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verificar se o endereço não foi encontrado
        if (addressToDelete == null)
        {
            return NotFound();
        }

        // Remover o endereço da lista de endereços do cliente
        customerFromDataBase.Addresses.Remove(addressToDelete);

        // Retornar uma resposta HTTP 204 No Content para indicar que o endereço foi removido com sucesso
        return NoContent();
    }

    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAddresses(int customerId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verificar se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound(); // Retorna o status HTTP 404 Not Found se o cliente não foi encontrado
        }

        var addressesToReturn = new List<AddressDto>();

        // Percorrer os endereços do cliente encontrado
        foreach (var address in customerFromDataBase.Addresses)
        {
            // Mapear os atributos do endereço para o formato AddressDto
            addressesToReturn.Add(new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City
            });
        }

        return Ok(addressesToReturn); // Retorna a lista de endereços com o status HTTP 200 OK
    }

    [HttpPost("create-with-addresses")]
    public ActionResult<CustomerWithAddressesDto> CreateCustomerWithAddresses([FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Cria uma nova entidade de cliente
        var customerEntity = new Customer
        {
            Id = Data.Instance.Customers.Max(c => c.Id) + 1,
            Name = customerWithAddressesCreateDto.Name,
            Cpf = customerWithAddressesCreateDto.Cpf
        };

        // Cria entidades de endereço para o cliente
        var addressEntities = new List<Address>();

        // Obtém o maior ID de todos os endereços existentes
        var maxAddressId = Data.Instance.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

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

        // Adiciona o cliente à fonte de dados (Data.Instance)
        Data.Instance.Customers.Add(customerEntity);

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

        return CreatedAtAction("GetAddresses", new { customerId = customerWithAddressesToReturn.Id }, customerWithAddressesToReturn);
    }

    [HttpPut("update-with-addresses")]
    public IActionResult UpdateCustomerWithAddresses(int customerId, [FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Verifica se o cliente existe
        var customerFromDataBase = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

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
        var maxAddressId = Data.Instance.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

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