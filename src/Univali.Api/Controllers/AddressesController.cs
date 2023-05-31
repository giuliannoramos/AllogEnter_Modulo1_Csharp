using Microsoft.AspNetCore.Mvc;
using Univali.Api.Dtos;
using Univali.Api.Entities;

namespace Univali.Api.Controller;

[ApiController]
[Route("api/addresses")]

public class AddressesController : ControllerBase
{
    /// <summary>
    /// Obtém a lista de todos os endereços cadastrados juntamente com os nomes dos clientes.
    /// </summary>
    /// <returns>Uma lista de endereços com os nomes dos clientes.</returns>
    [HttpGet]
    public IActionResult GetAllAddresses()
    {
        // Obtém a lista de endereços
        var addresses = Data.Instance.Addresses;

        // Cria uma lista para armazenar os endereços com nomes de clientes
        var addressesWithNames = new List<CustomerAddressDto>();

        // Percorre por cada endereço na lista de endereços 
        foreach (var address in addresses)
        {
            // Procura o cliente que possui o mesmo ID do cliente associado ao endereço atual
            var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == address.CustomerId);

            // Retorna NotFound se o cliente não foi encontrado
            if (customer == null)
            {
                return NotFound();
            }

            // Adiciona o endereço com o nome do cliente à lista
            addressesWithNames.Add(new CustomerAddressDto
            {
                CustomerName = customer.Name,
                Street = address.Street,
                City = address.City,
                State = address.State

            });
        }

        return Ok(addressesWithNames);
    }

    /// <summary>
    /// Obtém um endereço específico com base no ID.
    /// </summary>
    /// <param name="addressId">O ID do endereço.</param>
    /// <returns>O endereço correspondente ao ID fornecido.</returns>
    [HttpGet("{addressId}", Name = "GetAddressById")]
    public IActionResult GetAddressById(int addressId)
    {
        // Obtém o endereço com base no ID fornecido
        var address = Data.Instance.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (address == null)
        {
            return NotFound();
        }

        // Procura o cliente que possui o mesmo ID do cliente associado ao endereço atual
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == address.CustomerId);

        // Retorna NotFound se o cliente não foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Cria um objeto CustomerAddressDto com o nome do cliente e as informações do endereço atual
        var addressWithName = new CustomerAddressDto
        {
            CustomerName = customer.Name,
            Street = address.Street,
            City = address.City,
            State = address.State
        };

        return Ok(addressWithName);
    }

    /// <summary>
    /// Cria um novo endereço.
    /// </summary>
    /// <param name="addressDto">O objeto contendo as informações do novo endereço.</param>
    /// <returns>Um IActionResult indicando o resultado da criação.</returns>
    [HttpPost]
    public IActionResult CreateAddress(AddressDto addressDto)
    {
        // Verifica se o cliente associado ao endereço existe
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == addressDto.CustomerId);

        // Retorna NotFound se o cliente não foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Cria um novo objeto endereço com as informações fornecidas
        var address = new Address
        {
            CustomerId = addressDto.CustomerId,
            Street = addressDto.Street,
            City = addressDto.City,
            State = addressDto.State
        };

        // Adiciona o novo endereço à lista de endereços
        Data.Instance.AddAddress(addressDto);

        // Retorna um objeto CustomerAddressDto com o nome do cliente e as informações do novo endereço
        var customerAddressDtoResponse = new CustomerAddressDto
        {
            CustomerName = customer.Name,
            Street = address.Street,
            City = address.City,
            State = address.State
        };

        return CreatedAtAction("GetAddressById", new { addressId = address.Id }, customerAddressDtoResponse);
    }

    /// <summary>
    /// Atualiza um endereço específico com base no ID.
    /// </summary>
    /// <param name="addressId">O ID do endereço a ser atualizado.</param>
    /// <param name="updatedAddress">O objeto contendo as informações atualizadas do endereço.</param>
    /// <returns>Um IActionResult indicando o resultado da atualização.</returns>
    [HttpPut("{addressId}")]
    public IActionResult UpdateAddressById(int addressId, AddressDto updatedAddress)
    {
        // Obtém o endereço com base no ID fornecido
        var address = Data.Instance.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (address == null)
        {
            return NotFound();
        }

        // Procura o cliente que possui o mesmo ID do cliente associado ao endereço atual
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == address.CustomerId);

        // Retorna NotFound se o cliente não foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Atualiza as informações do endereço com as informações fornecidas
        address.Street = updatedAddress.Street;
        address.City = updatedAddress.City;
        address.State = updatedAddress.State;

        // Cria um objeto CustomerAddressDto com o nome do cliente e as informações do endereço atualizado
        var addressWithName = new CustomerAddressDto
        {
            CustomerName = customer.Name,
            Street = address.Street,
            City = address.City,
            State = address.State
        };

        return Ok(addressWithName);
    }

    /// <summary>
    /// Exclui um endereço específico com base no ID.
    /// </summary>
    /// <param name="addressId">O ID do endereço a ser excluído.</param>
    /// <returns>Um IActionResult indicando o resultado da exclusão.</returns>
    [HttpDelete("{addressId}")]
    public IActionResult DeleteAddressById(int addressId)
    {
        // Obtém o endereço com base no ID fornecido
        var address = Data.Instance.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (address == null)
        {
            return NotFound();
        }

        // Remove o endereço da lista de endereços
        Data.Instance.Addresses.Remove(address);

        return NoContent();
    }

}