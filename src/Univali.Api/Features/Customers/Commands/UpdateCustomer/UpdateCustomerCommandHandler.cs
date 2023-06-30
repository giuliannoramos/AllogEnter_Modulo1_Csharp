using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IUpdateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task HandleUpdate(UpdateCustomerCommand request)
        {
            // Obtém a entidade do cliente pelo ID
            // ?? é uma expressão para simplificar uma estrutura de if caso customer seja nulo, retornar o not found
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(request.Id) ?? throw new InvalidOperationException("Customer not found");

            // Mapeia as propriedades do comando de atualização para a entidade do cliente
            _mapper.Map(request, customerEntity);

            // Salva as alterações no repositório
            await _customerRepository.SaveChangesAsync();           
        }
    }
}
