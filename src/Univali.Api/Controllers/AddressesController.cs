using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAddresses(int customerId)
    {
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == customerId);

        if (customerFromDatabase == null) return NotFound();

        var addressesToReturn = new List<AddressDto>();

        foreach (var address in customerFromDatabase.Addresses)
        {
            addressesToReturn.Add(new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City
            });
        }

        return Ok(addressesToReturn);

    }

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        // // Procurar o cliente com o ID fornecido
        // var customer = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == customerId);

        // // Verificar se o cliente foi encontrado
        // if (customer != null)
        // {
        //     // Procurar o endereço com o ID fornecido dentro do cliente encontrado
        //     var addressToReturn = customer.Addresses.FirstOrDefault(address => address.Id == addressId);

        //     // Verificar se o endereço foi encontrado
        //     if (addressToReturn != null)
        //     {
        //         // Retornar o endereço encontrado com o status HTTP 200 OK
        //         return Ok(addressToReturn);
        //     }
        // }

        // // Se o cliente ou o endereço não forem encontrados, retornar o status HTTP 404 Not Found
        // return NotFound();

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
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customer == null)
            return NotFound();

        // Cria uma nova entidade de endereço
        var addressEntity = new Address()
        {
            // Gera o ID para o novo endereço
            Id = customer.Addresses.SelectMany<Address, int>(a => new[] { a.Id }) // Combina todos os IDs de endereços existentes em uma única sequência
                 .DefaultIfEmpty(0) // Define o valor padrão como 0 caso a sequência esteja vazia (nenhum endereço existente)                
                 .Max() + 1, // Encontra o maior ID na sequência e adiciona 1 para gerar o próximo ID disponível
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        // Adiciona o novo endereço à lista de endereços do cliente
        customer.Addresses.Add(addressEntity);

        // Retorna a resposta de criação com o status 201 (Created)
        return CreatedAtAction("GetAddress", new { customerId = customerId, addressId = addressEntity.Id }, new AddressDto
        {
            // Mapeia os atributos do novo endereço para o DTO de resposta
            Id = addressEntity.Id,
            Street = addressEntity.Street,
            City = addressEntity.City
        });
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateAddress(int customerId, int addressId, [FromBody] AddressForUpdateDto addressForUpdateDto)
    {
        // Verificar se o ID do endereço fornecido no corpo da requisição corresponde ao ID fornecido como parâmetro
        if (addressId != addressForUpdateDto.Id) return BadRequest();

        // Procura o cliente com o ID fornecido
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customer == null)
            return NotFound();

        // Procura o endereço com o ID fornecido dentro do cliente
        var addressToUpdate = customer.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (addressToUpdate == null)
            return NotFound();

        // Atualiza os atributos do endereço com base nos dados fornecidos
        addressToUpdate.Street = addressForUpdateDto.Street;
        addressToUpdate.City = addressForUpdateDto.City;

        // Retorna o status 204 No Content para indicar que a atualização foi bem-sucedida
        return NoContent();
    }

}