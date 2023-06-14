namespace Univali.Api.Features.Customers.Queries.GetAllCustomers;


public interface IGetAllCustomersQueryHandler
{
   Task<IEnumerable<GetAllCustomersDto>> HandleGetAll();
}