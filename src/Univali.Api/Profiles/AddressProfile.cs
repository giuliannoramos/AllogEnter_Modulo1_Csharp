using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    { 
        // Mapeamento para Retornar:
        CreateMap<Address, AddressDto>(); // Mapeia um objeto Address do banco para retornar um AddressDto        

        // Mapeamento para Salvar:        
        CreateMap<AddressDto, Address>(); // Mapeia um AddressDto para salvar no banco como Address
        CreateMap<AddressForCreationDto, Address>(); // Mapeia um AddressForCreationDto para salvar no banco como Address

        // Mapeamento para Atualizar:
        CreateMap<AddressForUpdateDto, Address>(); // Mapeia mudanças do AddressForUpdateDto para atualizar o objeto Address no banco
    }
}