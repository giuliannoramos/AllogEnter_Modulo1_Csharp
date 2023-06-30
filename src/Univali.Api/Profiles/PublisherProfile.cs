using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Publishers.Commands.CreatePublisher;
using Univali.Api.Features.Publishers.Queries.GetPublisherById;

namespace Univali.Api.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<CreatePublisherCommand, Publisher>();
            CreateMap<Publisher, CreatePublisherDto>();

            CreateMap<Publisher, GetPublisherByIdDto>();

        }
    }
}