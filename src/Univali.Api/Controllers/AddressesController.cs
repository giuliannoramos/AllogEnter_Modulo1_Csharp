using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : ControllerBase
{
    // Injeção de Dependência: Os parâmetros 'data' e 'mapper' são fornecidos externamente para o construtor da classe CustomersController.
    // Isso permite que as dependências necessárias sejam injetadas na classe em vez de a própria classe criar essas dependências.
    private readonly Data _data;
    private readonly IMapper _mapper;

    public AddressesController(Data data, IMapper mapper)
    {
        // Armazena uma referência aos dados fornecidos externamente, como um banco de dados.
        _data = data ?? throw new ArgumentNullException(nameof(data));

        // Armazena uma referência ao objeto responsável por mapear diferentes tipos de objetos, como mapear um Customer para CustomerDto
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        // // Procurar o cliente com o ID fornecido
        // var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

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
            var addressToReturn = _data
                .Customers.FirstOrDefault(customer => customer.Id == customerId)
                ?.Addresses.FirstOrDefault(address => address.Id == addressId);

            return addressToReturn != null ? Ok(addressToReturn) : NotFound();
        }
    }

    [HttpPost]
    public ActionResult<AddressDto> CreateAddress(int customerId, [FromBody] AddressForCreationDto addressForCreationDto)
    {
        // Procura o cliente com o ID fornecido
        var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Obtém o maior ID de todos os endereços existentes
        var maxAddressId = _data.Customers.SelectMany(customer => customer.Addresses).Max(address => address.Id);

        // Cria uma nova entidade de endereço
        var addressEntity = _mapper.Map<Address>(addressForCreationDto);
        addressEntity.Id = maxAddressId + 1;

        // Adiciona o novo endereço à lista de endereços do cliente
        customerFromDataBase.Addresses.Add(addressEntity);

        // Mapeia os atributos do novo endereço para o DTO de resposta
        var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

        return CreatedAtAction("GetAddress", new { customerId = customerId, addressId = addressToReturn.Id }, addressToReturn);
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateAddress(int customerId, int addressId, [FromBody] AddressForUpdateDto addressForUpdateDto)
    {
        // Verifica se o ID do endereço fornecido no corpo da requisição corresponde ao ID fornecido como parâmetro
        if (addressId != addressForUpdateDto.Id)
            return BadRequest();

        // Procura o cliente com o ID fornecido
        var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
            return NotFound();

        // Procura o endereço com o ID fornecido dentro do cliente
        var addressToUpdate = customerFromDataBase.Addresses.FirstOrDefault(a => a.Id == addressId);

        // Verifica se o endereço não foi encontrado
        if (addressToUpdate == null)
            return NotFound();

        // Atualiza os atributos do endereço com base nos dados fornecidos
        _mapper.Map(addressForUpdateDto, addressToUpdate);

        // Retorna o status 204 No Content para indicar que a atualização foi bem-sucedida
        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

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
        var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

        // Verificar se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound(); // Retorna o status HTTP 404 Not Found se o cliente não foi encontrado
        }

        // Mapear os endereços do cliente para o formato AddressDto
        var addressesToReturn = _mapper.Map<List<AddressDto>>(customerFromDataBase.Addresses);

        return Ok(addressesToReturn); // Retorna a lista de endereços com o status HTTP 200 OK
    }

}