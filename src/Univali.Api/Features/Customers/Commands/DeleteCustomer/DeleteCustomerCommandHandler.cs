using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IDeleteCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task HandleDelete(DeleteCustomerCommand request)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(request.Id);

            if (customerEntity == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            _customerRepository.DeleteCustomer(customerEntity);
            await _customerRepository.SaveChangesAsync();
        }
    }
}