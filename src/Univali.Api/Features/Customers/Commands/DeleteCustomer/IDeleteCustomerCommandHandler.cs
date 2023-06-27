using System.Threading.Tasks;

namespace Univali.Api.Features.Customers.Commands.DeleteCustomer
{
    public interface IDeleteCustomerCommandHandler
    {
        Task HandleDelete(DeleteCustomerCommand request);
    }
}