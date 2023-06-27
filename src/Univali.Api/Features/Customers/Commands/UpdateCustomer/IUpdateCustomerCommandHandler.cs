namespace Univali.Api.Features.Customers.Commands.UpdateCustomer
{
    public interface IUpdateCustomerCommandHandler
    {
        Task HandleUpdate(UpdateCustomerCommand request);
    }
}