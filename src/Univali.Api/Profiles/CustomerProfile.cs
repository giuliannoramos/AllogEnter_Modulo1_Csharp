using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Mapeamento para Retornar:
            CreateMap<Customer, CustomerDto>(); // Mapeia um Customer do banco para retornar um CustomerDto
            CreateMap<Customer, CustomerWithAddressesDto>(); // Mapeia um Customer do banco para retornar um CustomerWithAddressesDto

            // Mapeamento para Salvar:
            CreateMap<CustomerForCreationDto, Customer>(); // Mapeia um CustomerForCreationDto para salvar no Customer do banco        
            CreateMap<CustomerWithAddressesCreateDto, Customer>(); // Mapeia um CustomerWithAddressesCreateDto para salvar no Customer do banco            

            // Mapeamento para Atualizar:
            CreateMap<CustomerForUpdateDto, Customer>(); // Mapeia mudanças do CustomerForUpdateDto para salvar no Customer do banco
            CreateMap<CustomerForPatchDto, Customer>(); // Mapeia mudanças do CustomerForPatchDto para salvar no Customer do banco
            CreateMap<Customer, CustomerForPatchDto>(); // Mapeia um objeto Customer do banco de dados para CustomerForPatchDto, permitindo atualizações parciais.            
        }
    }
}