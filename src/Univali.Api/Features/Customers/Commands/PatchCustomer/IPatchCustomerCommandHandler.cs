using System.Threading.Tasks;

namespace Univali.Api.Features.Customers.Commands.PatchCustomer
{
    public interface IPatchCustomerCommandHandler
    {
        Task HandlePatch(PatchCustomerCommand request);
    }
}