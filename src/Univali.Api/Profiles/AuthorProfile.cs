using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Publishers.Commands.CreateAuthor;
using Univali.Api.Features.Publishers.Commands.UpdateAuthor;
using Univali.Api.Features.Publishers.Queries.GetAuthorById;

using Univali.Api.Models;

namespace Univali.Api.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<CreateAuthorCommand, Author>();
            CreateMap<Author, CreateAuthorDto>();

            CreateMap<Author, GetAuthorByIdDto>();

            CreateMap<UpdateAuthorCommand, Author>();
        }
    }
}