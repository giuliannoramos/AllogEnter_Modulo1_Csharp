using Microsoft.AspNetCore.JsonPatch;
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
        var addressesWithNames = new List<AddressReturnDto>();

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
            addressesWithNames.Add(new AddressReturnDto
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                State = address.State,
                CustomerId = customer.Id,
                CustomerName = customer.Name
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
        var addressWithName = new AddressReturnDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            CustomerId = customer.Id,
            CustomerName = customer.Name
        };

        return Ok(addressWithName);
    }


    /// <summary>
    /// Obtém todos os endereços de um cliente com base no ID do cliente.
    /// </summary>
    /// <param name="customerId">O ID do cliente.</param>
    /// <returns>Uma lista de endereços associados ao cliente.</returns>
    [HttpGet("{customerId}/addresses")]
    public IActionResult GetAddressesByCustomerId(int customerId)
    {
        // Procura o cliente com base no ID fornecido
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            return NotFound();
        }

        // Obtém todos os endereços associados ao cliente
        var addresses = Data.Instance.Addresses.Where(a => a.CustomerId == customerId);

        // Retorna NotFound se nenhum endereço foi encontrado para o cliente
        if (!addresses.Any())
        {
            return NotFound();
        }

        // Cria uma lista de objetos AddressReturnDto com as informações dos endereços
        var addressesWithName = addresses.Select(address => new AddressReturnDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            CustomerId = customer.Id,
            CustomerName = customer.Name
        }).ToList();

        return Ok(addressesWithName);
    }


    /// <summary>
    /// Cria um novo endereço.
    /// </summary>
    /// <param name="addressDto">O objeto contendo as informações do novo endereço.</param>
    /// <returns>Um IActionResult indicando o resultado da criação.</returns>
    [HttpPost]
    public IActionResult CreateAddress(AddressCreateDto addressCreateDto)
    {
        // Verifica se o cliente associado ao endereço existe
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == addressCreateDto.CustomerId);

        // Retorna NotFound se o cliente não foi encontrado
        if (customer == null)
        {
            return NotFound();
        }

        // Cria um novo objeto endereço com as informações fornecidas
        var address = new Address
        {
            Id = Data.Instance.GenerateAddressId(),
            CustomerId = addressCreateDto.CustomerId,
            Street = addressCreateDto.Street,
            City = addressCreateDto.City,
            State = addressCreateDto.State
        };

        // Adiciona o novo endereço à lista de endereços
        Data.Instance.AddAddress(address);

        // Retorna um objeto CustomerAddressDto com o nome do cliente e as informações do novo endereço
        var addressReturnDto = new AddressReturnDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            CustomerId = customer.Id,
            CustomerName = customer.Name
        };

        return CreatedAtAction("GetAddressById", new { addressId = address.Id }, addressReturnDto);
    }


    /// <summary>
    /// Atualiza um endereço específico com base no ID.
    /// </summary>
    /// <param name="addressId">O ID do endereço a ser atualizado.</param>
    /// <param name="updatedAddress">O objeto contendo as informações atualizadas do endereço.</param>
    /// <returns>Um IActionResult indicando o resultado da atualização.</returns>
    [HttpPut("{addressId}")]
    public IActionResult UpdateAddressById(int addressId, AddressCreateDto addressCreateDto)
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
        address.Street = addressCreateDto.Street;
        address.City = addressCreateDto.City;
        address.State = addressCreateDto.State;

        // Retorna um código de status HTTP 204 (No Content) para indicar sucesso na atualização sem retornar dados adicionais
        return NoContent();
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

    /// <summary>
    /// Método para atualizar parcialmente um Endereço.
    /// </summary>
    /// <param name="patchDocument">Documento JSON com as alterações a serem aplicadas no endereço.</param>
    /// <param name="id">O ID do endereço a ser atualizado.</param>
    /// <returns>Um código de status HTTP indicando o sucesso ou fracasso da operação.</returns>
    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateAddress(
    [FromBody] JsonPatchDocument<AddressCreateDto> patchDocument,
    [FromRoute] int id)
    {
        // Obter o cliente do banco de dados com base no ID fornecido.
        var addressFromDataBase = Data.Instance.Addresses
            .FirstOrDefault(address => address.Id == id);

        // Verificar se o cliente foi encontrado.
        if (addressFromDataBase == null)
            return NotFound();

        // Criar um objeto CustomerCreateDto com as propriedades do cliente a serem atualizadas.
        var addressToPatch = new AddressCreateDto
        {
            Street = addressFromDataBase.Street,
            City = addressFromDataBase.City,
            State = addressFromDataBase.State
        };

        // Aplicar as alterações do patchDocument no customerToPatch.
        patchDocument.ApplyTo(addressToPatch);

        // Atualizar as propriedades do cliente no banco de dados com base nas alterações aplicadas.
        addressFromDataBase.Street = addressToPatch.Street;
        addressFromDataBase.City = addressToPatch.City;
        addressFromDataBase.State = addressToPatch.State;

        // Retornar um resultado sem conteúdo para indicar que a atualização parcial foi bem-sucedida.
        return NoContent();
    }

}