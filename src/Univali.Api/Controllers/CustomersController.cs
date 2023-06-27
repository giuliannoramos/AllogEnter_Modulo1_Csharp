using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Univali.Api.DbContexts;
using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Commands.DeleteCustomer;
using Univali.Api.Features.Customers.Commands.PatchCustomer;
using Univali.Api.Features.Customers.Commands.UpdateCustomer;
using Univali.Api.Features.Customers.Queries.GetAllCustomers;
using Univali.Api.Features.Customers.Queries.GetCustomerCpf;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;
using Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : MainController
{
    // Injeção de Dependência: Os parâmetros 'data' e 'mapper' são fornecidos externamente para o construtor da classe CustomersController.
    // Isso permite que as dependências necessárias sejam injetadas na classe em vez de a própria classe criar essas dependências.
    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;

    public CustomersController(Data data, IMapper mapper, CustomerContext context,
        ICustomerRepository customerRepository, IMediator mediator)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers([FromServices] IGetAllCustomersQueryHandler handler)
    {
        // Chama o manipulador (handler) responsável por lidar com a consulta GetCustomerDetailQuery
        // e aguarda o resultado (await) da execução assíncrona.
        var customersToReturn = await handler.HandleGetAll();

        // Retorna uma resposta "OK" (200) com os clientes mapeados para o tipo CustomerDto
        return Ok(customersToReturn);
    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById([FromServices] IGetCustomerDetailQueryHandler handler, int customerId)
    {
        // Cria uma nova instância do objeto GetCustomerDetailQuery com o ID do cliente fornecido
        var getCustomerDetailQuery = new GetCustomerDetailQuery { Id = customerId };

        // Chama o manipulador (handler) responsável por lidar com a consulta GetCustomerDetailQuery
        // e aguarda o resultado (await) da execução assíncrona.
        var customerToReturn = await handler.Handle(getCustomerDetailQuery);

        // Verifica se o cliente retornado é nulo. Se for, retorna uma resposta HTTP 404 (NotFound).
        if (customerToReturn == null)
        {
            return NotFound();
        }

        // Se o cliente existir, retorna uma resposta HTTP 200 (Ok) contendo o cliente retornado.
        return Ok(customerToReturn);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByCpf(string cpf, [FromServices] IGetCustomerCpfQueryHandler handler)
    {
        // Cria uma nova instância do objeto GetCustomerCpfQuery com o CPF do cliente fornecido
        var getCustomerCpfQuery = new GetCustomerCpfQuery { Cpf = cpf };

        // Chama o manipulador (handler) responsável por lidar com a consulta GetCustomerCpfQuery
        // e aguarda o resultado (await) da execução assíncrona.
        var customerToReturn = await handler.HandleGetCpf(getCustomerCpfQuery);

        // Verifica se o cliente retornado é nulo. Se for, retorna uma resposta HTTP 404 (NotFound).
        if (customerToReturn == null)
        {
            return NotFound();
        }

        // Se o cliente existir, retorna uma resposta HTTP 200 (Ok) contendo o cliente retornado.
        return Ok(customerToReturn);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerCommand createCustomerCommand, [FromServices] ICreateCustomerCommandHandler handler)
    {
        // Chama o manipulador (handler) responsável por lidar com o comando CreateCustomerCommand
        // e aguarda o resultado (await) da execução assíncrona.
        var customerToReturn = await handler.Handle(createCustomerCommand);

        return CreatedAtRoute("GetCustomerById", new { customerId = customerToReturn.Id }, customerToReturn);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, UpdateCustomerCommand updateCustomerCommand, [FromServices] IUpdateCustomerCommandHandler handler)
    {
        // Verifica se o ID fornecido corresponde ao ID no objeto UpdateCustomerCommand
        if (id != updateCustomerCommand.Id)
        {
            // Se os IDs não corresponderem, retorna um resultado "Bad Request" (400)
            return BadRequest();
        }

        // Chama o manipulador responsável por atualizar o cliente
        await handler.HandleUpdate(updateCustomerCommand);

        // Retorna um resultado "Sem conteúdo" (204) para indicar que o cliente foi atualizado com sucesso
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id, [FromServices] IDeleteCustomerCommandHandler handler)
    {
        var deleteCustomerCommand = new DeleteCustomerCommand { Id = id };

        try
        {
            await handler.HandleDelete(deleteCustomerCommand);
        }
        catch (InvalidOperationException)
        {
            return NotFound(); // Cliente não encontrado
        }

        return NoContent(); // Cliente excluído com sucesso
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PartiallyUpdateCustomer([FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id)
    {
        // Encontra o cliente no banco de dados com base no ID fornecido
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(id);

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
    public async Task<ActionResult<IEnumerable<CustomerWithAddressesDto>>> GetCustomersWithAddresses()
    {
        // Obtém os clientes do banco de dados com os endereços associados
        var customersFromDatabase = await _customerRepository.GetCustomersWithAddressesAsync();

        // Mapeia a lista de clientes para a lista de CustomerWithAddressesDto
        var customersToReturn = _mapper.Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        // Retorna uma resposta HTTP 200 OK com a lista de clientes com endereços
        return Ok(customersToReturn);
    }

    [HttpGet("with-address/{id}")]
    public async Task<ActionResult<CustomerWithAddressesDto>> GetCustomerWithAddressesById([FromServices] IGetCustomerWithAddressesQueryHandler handler, int customerId)
    {
        var getCustomerWithAddressesQuery = new GetCustomerWithAddressesQuery { Id = customerId };

        var customerToReturn = await handler.HandleCustomerWithAddresses(getCustomerWithAddressesQuery);

        if (customerToReturn == null)
        {
            return NotFound();
        }

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
    public async Task<IActionResult> UpdateCustomerWithAddresses(int customerId, [FromBody] CustomerWithAddressesCreateDto customerWithAddressesCreateDto)
    {
        // Seleciona o cliente pelo Id
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(customerId);

        // Verifica se existe
        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        // Atualiza os dados do cliente        
        _mapper.Map(customerWithAddressesCreateDto, customerFromDatabase);

        // Salva no banco
        _context.SaveChanges();

        // Retorna 204 indicando que foi atualizado
        return NoContent();
    }

}