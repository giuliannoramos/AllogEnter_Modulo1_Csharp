using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetAddresses;
public class GetAddressesQueryHandler : IGetAddressesQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAddressesQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }   

    public async Task<List<GetAddressesDto>> HandleGetAddresses(GetAddressesQuery request)
    {
        var addressesFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(request.CustomerId);
        return _mapper.Map<List<GetAddressesDto>>(addressesFromDatabase);
    }
}