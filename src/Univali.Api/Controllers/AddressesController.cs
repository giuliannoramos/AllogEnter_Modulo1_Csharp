using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.DbContexts;
using Microsoft.EntityFrameworkCore;
using Univali.Api.Repositories;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController
{
    // Injeção de Dependência: Os parâmetros 'data' e 'mapper' são fornecidos externamente para o construtor da classe CustomersController.
    // Isso permite que as dependências necessárias sejam injetadas na classe em vez de a própria classe criar essas dependências.
    // Injeção de Dependência: Os parâmetros 'data' e 'mapper' são fornecidos externamente para o construtor da classe CustomersController.
    // Isso permite que as dependências necessárias sejam injetadas na classe em vez de a própria classe criar essas dependências.
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public AddressesController(Data data, IMapper mapper, CustomerContext context, ICustomerRepository customerRepository)
    {
        // Armazena uma referência aos dados fornecidos externamente, como um banco de dados.
        _data = data ?? throw new ArgumentNullException(nameof(data));

        // Armazena uma referência ao objeto responsável por mapear entre diferentes tipos de objetos, como mapear Customer para CustomerDto
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _context = context ?? throw new ArgumentNullException(nameof(_context));

        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("{addressId}")]
    public async Task<ActionResult<AddressDto>> GetAddress(int customerId, int addressId)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(customerId);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        var addressToReturn = await _customerRepository.GetAddressAsync(addressId);

        if (addressToReturn == null)
        {
            return NotFound();
        }

        var addressDto = _mapper.Map<AddressDto>(addressToReturn);

        return Ok(addressDto);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddress(int customerId, [FromBody] AddressForCreationDto addressForCreationDto)
    {
        // Procura o cliente com o ID fornecido
        var customerFromDataBase = await _customerRepository.GetCustomerByIdAsync(customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        // Cria uma nova entidade de endereço
        var addressEntity = _mapper.Map<Address>(addressForCreationDto);

        // Adiciona o novo endereço à lista de endereços do cliente
        customerFromDataBase.Addresses.Add(addressEntity);
        _context.SaveChanges();

        // Mapeia os atributos do novo endereço para o DTO de resposta
        var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

        return CreatedAtAction("GetAddress", new { customerId = customerId, addressId = addressToReturn.Id }, addressToReturn);
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult> UpdateAddress(int customerId, int addressId, AddressForUpdateDto addressForUpdateDto)
    {
        if (addressForUpdateDto.Id != addressId) return BadRequest();

        // Procura o cliente com o ID fornecido
        var customerFromDataBase = await _customerRepository.GetCustomerByIdAsync(customerId);

        // Verifica se o cliente não foi encontrado
        if (customerFromDataBase == null)
        {
            return NotFound();
        }

        var addressFromDatabase = await _customerRepository.GetAddressAsync(addressId);

        if (addressFromDatabase == null)
        {
            return NotFound();
        }

        _mapper.Map(addressForUpdateDto, addressFromDatabase);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult> DeleteAddress(int customerId, int addressId)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(customerId);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        var addressToDelete = await _customerRepository.GetAddressAsync(addressId);

        if (addressToDelete == null)
        {
            return NotFound();
        }

        // Remover o endereço da lista de endereços do cliente
        customerFromDatabase.Addresses.Remove(addressToDelete);
        _context.SaveChanges();

        // Retornar uma resposta HTTP 204 No Content para indicar que o endereço foi removido com sucesso
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddresses(int customerId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(customerId);

        // Verificar se o cliente não foi encontrado
        if (customerFromDatabase == null)
        {
            return NotFound(); // Retorna o status HTTP 404 Not Found se o cliente não foi encontrado
        }

        // Mapear os endereços do cliente para o formato AddressDto
        var addressesToReturn = _mapper.Map<List<AddressDto>>(customerFromDatabase.Addresses);

        return Ok(addressesToReturn); // Retorna a lista de endereços com o status HTTP 200 OK
    }

}