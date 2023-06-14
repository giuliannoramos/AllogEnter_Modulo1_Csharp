using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomerWithAddresses;

public class GetCustomerWithAddressesQueryHandler : IGetCustomerWithAddressesQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerWithAddressesQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerWithAddressesDto?> HandleCustomerWithAddresses(GetCustomerWithAddressesQuery request)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(request.Id);
        return _mapper.Map<GetCustomerWithAddressesDto>(customerFromDatabase);
    }
}