using AutoMapper;

namespace Univali.Api.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Entities.Customer, Models.CustomerDto>();
    }
}