using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetAllCustomers;


public class GetAllCustomersQueryHandler : IGetAllCustomersQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<GetAllCustomersDto>> HandleGetAll()
    {
        // Obtém todos os clientes do banco de dados
        var customersFromDatabase = await _customerRepository.GetCustomersAsync();

        // Mapeia os clientes para o tipo CustomerDto usando o AutoMapper
        return _mapper.Map<IEnumerable<GetAllCustomersDto>>(customersFromDatabase);
    }
}