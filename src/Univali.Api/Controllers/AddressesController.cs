using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;
using Univali.Api.DbContexts;
using Microsoft.EntityFrameworkCore;

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

    public AddressesController(Data data, IMapper mapper, CustomerContext context)
    {
        // Armazena uma referência aos dados fornecidos externamente, como um banco de dados.
        _data = data ?? throw new ArgumentNullException(nameof(data));

        // Armazena uma referência ao objeto responsável por mapear entre diferentes tipos de objetos, como mapear Customer para CustomerDto
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _context = context ?? throw new ArgumentNullException(nameof(_context));
    }


    // [HttpGet("{addressId}")]
    // public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    // {
    //     // // Procurar o cliente com o ID fornecido
    //     // var customerFromDataBase = _data.Customers.FirstOrDefault(c => c.Id == customerId);

    //     // // Verificar se o cliente não foi encontrado
    //     // if (customerFromDataBase == null)
    //     // {
    //     //     return NotFound();
    //     // }

    //     // // Procurar o endereço com o ID fornecido dentro do cliente encontrado
    //     // var addressToReturn = customerFromDataBase.Addresses.FirstOrDefault(a => a.Id == addressId);

    //     // // Verificar se o endereço não foi encontrado
    //     // if (addressToReturn == null)
    //     // {
    //     //     return NotFound();
    //     // }

    //     // // Retornar o endereço encontrado com o status HTTP 200 OK
    //     // return Ok(addressToReturn);

    //     {
    //         var addressToReturn = _data
    //             .Customers.FirstOrDefault(customer => customer.Id == customerId)
    //             ?.Addresses.FirstOrDefault(address => address.Id == addressId);

    //         return addressToReturn != null ? Ok(addressToReturn) : NotFound();
    //     }
    // }

    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        var customerFromDatabase = _context.Customers
        .Include(c => c.Addresses)
        .FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        var addressToReturn = customerFromDatabase.Addresses.FirstOrDefault(address => address.Id == addressId);

        if (addressToReturn == null)
        {
            return NotFound();
        }

        var addressDto = _mapper.Map<AddressDto>(addressToReturn);

        return Ok(addressDto);
    }



    [HttpPost]
    public ActionResult<AddressDto> CreateAddress(int customerId, [FromBody] AddressForCreationDto addressForCreationDto)
    {
        // Procura o cliente com o ID fornecido
        var customerFromDataBase = _context.Customers.FirstOrDefault(c => c.Id == customerId);

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
    public ActionResult UpdateAddress(int customerId, int addressId,
     AddressForUpdateDto addressForUpdateDto)
    {
        if (addressForUpdateDto.Id != addressId) return BadRequest();

        // Método Any retorna true ou false.
        // Se customer existe retorna true, se não existe retorna false.
        var customerExists = _context.Customers
            .Any(c => c.Id == customerId);

        if (!customerExists) return NotFound();

        var addressFromDatabase = _context.Addresses
            .FirstOrDefault(a => a.CustomerId == customerId && a.Id == addressId);

        if (addressFromDatabase == null) return NotFound();

        _mapper.Map(addressForUpdateDto, addressFromDatabase);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDataBase = _context.Customers.FirstOrDefault(c => c.Id == customerId);

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
        _context.SaveChanges();

        // Retornar uma resposta HTTP 204 No Content para indicar que o endereço foi removido com sucesso
        return NoContent();
    }

    [HttpGet]
    public ActionResult<IEnumerable<AddressDto>> GetAddresses(int customerId)
    {
        // Procurar o cliente com o ID fornecido
        var customerFromDatabase = _context.Customers
       .Include(c => c.Addresses)
       .FirstOrDefault(c => c.Id == customerId);

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