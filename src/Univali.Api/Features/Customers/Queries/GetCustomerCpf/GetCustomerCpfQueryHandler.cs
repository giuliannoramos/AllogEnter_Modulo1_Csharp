using AutoMapper;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Queries.GetCustomerCpf
{
    public class GetCustomerCpfQueryHandler : IGetCustomerCpfQueryHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerCpfQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomerCpfDto?> HandleGetCpf(GetCustomerCpfQuery request)
        {
            var customerFromDatabase = await _customerRepository.GetCustomerByCpfAsync(request.Cpf);
            return _mapper.Map<GetCustomerCpfDto>(customerFromDatabase);
        }
    }
}