using System;
using System.Threading.Tasks;
using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.PatchCustomer
{
    public class PatchCustomerCommandHandler : IPatchCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public PatchCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task HandlePatch(PatchCustomerCommand request)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(request.Id);

            if (customerEntity == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            var customerToPatch = _mapper.Map<Customer>(request);

            // Aplica as alterações parciais no cliente existente
            _mapper.Map(customerToPatch, customerEntity);

            await _customerRepository.SaveChangesAsync();
        }
    }
}