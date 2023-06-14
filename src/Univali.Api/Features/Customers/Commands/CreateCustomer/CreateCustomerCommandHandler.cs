using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : ICreateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CreateCustomerDto> Handle(CreateCustomerCommand request)
        {
            // Mapeia o objeto CreateCustomerCommand para uma entidade Customer usando o AutoMapper.
            var customerEntity = _mapper.Map<Customer>(request);

            // Adiciona o cliente ao repositório.
            _customerRepository.AddCustomer(customerEntity);

            // Salva as alterações no banco de dados.
            await _customerRepository.SaveChangesAsync();

            // Mapeia a entidade Customer para um objeto CreateCustomerDto usando o AutoMapper.
            var customerToReturn = _mapper.Map<CreateCustomerDto>(customerEntity);

            // Retorna o objeto CreateCustomerDto criado.
            return customerToReturn;
        }
    }
}