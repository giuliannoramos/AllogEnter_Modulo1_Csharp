using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : MainController
{
    // Injeção de Dependência: Os parâmetros 'data' e 'mapper' são fornecidos externamente para o construtor da classe CustomersController.
    // Isso permite que as dependências necessárias sejam injetadas na classe em vez de a própria classe criar essas dependências.
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;

    public CustomersController(Data data, IMapper mapper, CustomerContext context)
    {
        // Armazena uma referência aos dados fornecidos externamente, como um banco de dados.
        _data = data ?? throw new ArgumentNullException(nameof(data));

        // Armazena uma referência ao objeto responsável por mapear entre diferentes tipos de objetos, como mapear Customer para CustomerDto
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _context = context ?? throw new ArgumentNullException(nameof(_context));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        // Obtém os clientes do banco de dados
        var customersFromDatabase = _context.Customers.OrderBy(c => c.Name).ToList();

        // Mapeia os clientes para o tipo CustomerDto usando o AutoMapper
        var customersToReturn = _mapper.Map<IEnumerable<CustomerDto>>(customersFromDatabase);

        // Retorna uma resposta "OK" (200) com os clientes mapeados para o tipo CustomerDto
        return Ok(customersToReturn);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        // Encontra o cliente no banco de dados com base no ID fornecido
        var customerFromDatabase = _context.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente existe
        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Mapeia o customerFromDatabase para o tipo CustomerDto usando o AutoMapper
        var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);

        // Retorna uma resposta "OK" (200) com o customerToReturn
        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        // Encontra o cliente no banco de dados com base no CPF fornecido
        var customerFromDatabase = _context.Customers.FirstOrDefault(c => c.Cpf == cpf);

        // Verifica se o cliente existe
        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Mapeia o customerFromDatabase para o tipo CustomerDto usando o AutoMapper
        var customerToReturn = _mapper.Map<CustomerDto>(customerFromDatabase);

        // Retorna uma resposta "OK" (200) com o customerToReturn
        return Ok(customerToReturn);
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        // Mapeia o customerForCreationDto para um objeto Customer usando o AutoMapper
        var customerEntity = _mapper.Map<Customer>(customerForCreationDto);

        // Adiciona o cliente à coleção Customers no contexto do banco de dados
        _context.Customers.Add(customerEntity);
        _context.SaveChanges();

        // Mapeia o customerEntity para o tipo CustomerDto usando o AutoMapper
        var customerToReturn = _mapper.Map<CustomerDto>(customerEntity);

        // Retorna uma resposta "Created" (201) com a rota para o recurso criado
        return CreatedAtRoute("GetCustomerById", new { id = customerToReturn.Id }, customerToReturn);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id, CustomerForUpdateDto customerForUpdateDto)
    {
        // Verifica se o ID fornecido corresponde ao ID no objeto customerForUpdateDto
        if (id != customerForUpdateDto.Id) return BadRequest();

        // Encontra o cliente no banco de dados com base no ID fornecido
        var customerFromDatabase = _context.Customers
            .FirstOrDefault(customer => customer.Id == id);

        // Verifica se o cliente existe
        if (customerFromDatabase == null) return NotFound();

        // Mapeia as propriedades do customerForUpdateDto para o customerFromDatabase usando o AutoMapper
        _mapper.Map(customerForUpdateDto, customerFromDatabase);

        _context.SaveChanges();

        // Retorna uma resposta "Sem conteúdo" (204) para indicar que o cliente foi atualizado com sucesso
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(int id)
    {
        // Encontra o cliente no banco de dados com base no ID fornecido
        var customerFromDatabase = _context.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente existe
        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Remove o cliente da coleção Customers no contexto do banco de dados
        _context.Customers.Remove(customerFromDatabase);
        _context.SaveChanges();

        // Retorna uma resposta "Sem conteúdo" (204) para indicar que o cliente foi removido com sucesso
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer([FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id)
    {
        // Encontra o cliente no banco de dados com base no ID fornecido
        var customerFromDatabase = _context.Customers.FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente existe
        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Mapeia o cliente do banco de dados para o tipo CustomerForPatchDto usando o AutoMapper
        var customerToPatch = _mapper.Map<CustomerForPatchDto>(customerFromDatabase);

        // Aplica as alterações parciais do JsonPatchDocument ao objeto customerToPatch
        patchDocument.ApplyTo(customerToPatch, ModelState);

        if (!TryValidateModel(customerToPatch))
        {
            return ValidationProblem(ModelState);
        }

        // Mapeia as alterações aplicadas de volta para o objeto customerFromDatabase
        _mapper.Map(customerToPatch, customerFromDatabase);

        _context.SaveChanges();

        // Retorna um resultado de "Sem conteúdo" (204) para indicar que a atualização parcial foi concluída com sucesso
        return NoContent();
    }

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses()
    {
        // Obtém os clientes do banco de dados com os endereços associados
        var customersFromDatabase = _context.Customers.Include(c => c.Addresses);

        // Mapeia a lista de clientes para a lista de CustomerWithAddressesDto
        var customersToReturn = _mapper.Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        // Retorna uma resposta HTTP 200 OK com a lista de clientes com endereços
        return Ok(customersToReturn);
    }

    [HttpGet("with-address/{id}")]
    public ActionResult<CustomerWithAddressesDto> GetCustomerWithAddressesById(int id)
    {
        // Obtém o cliente específico do banco de dados com os endereços associados
        var customerFromDatabase = _context.Customers
            .Include(c => c.Addresses)
            .FirstOrDefault(c => c.Id == id);

        // Verifica se o cliente foi encontrado
        if (customerFromDatabase == null)
        {
            return NotFound(); // Retorna uma resposta HTTP 404 Not Found caso o cliente não seja encontrado
        }

        // Mapeia o cliente para o CustomerWithAddressesDto
        var customerToReturn = _mapper.Map<CustomerWithAddressesDto>(customerFromDatabase);

        // Retorna uma resposta HTTP 200 OK com o cliente e seus endereços
        return Ok(customerToReturn);
    }

    [HttpPost("with-addresses")]
    public ActionResult<CustomerWithAddressesDto> CreateCustomerWithAddresses([FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Mapeia o DTO de criação para a entidade de cliente
        var customerEntity = _mapper.Map<Customer>(customerWithAddressesCreateDto);

        // Adiciona o cliente à coleção Customers no contexto do banco de dados
        _context.Customers.Add(customerEntity);
        _context.SaveChanges();

        // Mapeia a entidade de cliente para o DTO a ser retornado
        var customerWithAddressesToReturn = _mapper.Map<CustomerWithAddressesDto>(customerEntity);

        return CreatedAtAction("GetCustomersWithAddresses", new { id = customerWithAddressesToReturn.Id }, customerWithAddressesToReturn);
    }

    [HttpPut("with-addresses/{customerId}")]
    public IActionResult UpdateCustomerWithAddresses(int customerId, [FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Verifica se o cliente existe
        var customerFromDatabase = _context.Customers.Include(c => c.Addresses).FirstOrDefault(c => c.Id == customerId);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Atualiza os dados do cliente
        _mapper.Map(customerWithAddressesCreateDto, customerFromDatabase);

        _context.SaveChanges();

        return NoContent();
    }
}